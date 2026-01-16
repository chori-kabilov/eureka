using Domain.Groups;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers.v1;

// Контроллер Dashboard для статистики
[ApiController]
[Route("api/v1/dashboard")]
[Authorize]
public class DashboardController(DataContext db) : ControllerBase
{
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats(CancellationToken ct)
    {
        var studentsCount = await db.StudentsSet.CountAsync(ct);
        var teachersCount = await db.TeachersSet.CountAsync(ct);
        var coursesCount = await db.CoursesSet.CountAsync(ct);
        var groupsCount = await db.GroupsSet.Where(g => g.Status == GroupStatus.Active).CountAsync(ct);

        return Ok(new
        {
            StudentsCount = studentsCount,
            TeachersCount = teachersCount,
            CoursesCount = coursesCount,
            GroupsCount = groupsCount
        });
    }

    [HttpGet("recent-enrollments")]
    public async Task<IActionResult> GetRecentEnrollments(CancellationToken ct)
    {
        var enrollments = await db.GroupEnrollmentsSet
            .Include(e => e.Student).ThenInclude(s => s!.User)
            .Include(e => e.Group).ThenInclude(g => g!.Course)
            .OrderByDescending(e => e.EnrolledAt)
            .Take(5)
            .Select(e => new
            {
                StudentName = e.Student != null ? e.Student.User!.FullName : "Неизвестно",
                CourseName = e.Group != null && e.Group.Course != null ? e.Group.Course.Name : "Неизвестно",
                TimeAgo = GetTimeAgo(e.EnrolledAt)
            })
            .ToListAsync(ct);

        return Ok(enrollments);
    }

    [HttpGet("popular-courses")]
    public async Task<IActionResult> GetPopularCourses(CancellationToken ct)
    {
        // Получаем популярные курсы через количество записей
        var courses = await db.CoursesSet
            .Select(c => new
            {
                c.Name,
                StudentsCount = db.GroupsSet
                    .Where(g => g.CourseId == c.Id && g.Status == GroupStatus.Active)
                    .SelectMany(g => db.GroupEnrollmentsSet.Where(e => e.GroupId == g.Id && e.Status == EnrollmentStatus.Active))
                    .Count()
            })
            .OrderByDescending(c => c.StudentsCount)
            .Take(5)
            .ToListAsync(ct);

        return Ok(courses);
    }

    private static string GetTimeAgo(DateTime date)
    {
        var diff = DateTime.UtcNow - date;
        if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes} мин. назад";
        if (diff.TotalHours < 24) return $"{(int)diff.TotalHours} ч. назад";
        if (diff.TotalDays < 7) return $"{(int)diff.TotalDays} дн. назад";
        return date.ToString("dd.MM.yyyy");
    }
}
