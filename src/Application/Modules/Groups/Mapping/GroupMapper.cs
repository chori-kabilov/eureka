using Application.Modules.Groups.Dtos;
using Domain.Groups;
using Domain.Schedule;
using Domain.Journal;
using Domain.Grading;
using Domain.Groups;

namespace Application.Modules.Groups.Mapping;

public static class GroupMapper
{
    public static GroupDto ToDto(Group group, int currentStudents = 0) => new()
    {
        Id = group.Id,
        Name = group.Name,
        Code = group.Code,
        CourseId = group.CourseId,
        CourseName = group.Course?.Name ?? string.Empty,
        ResponsibleTeacherId = group.ResponsibleTeacherId,
        ResponsibleTeacherName = group.ResponsibleTeacher?.User?.FullName ?? string.Empty,
        StartDate = group.StartDate,
        EndDate = group.EndDate,
        MaxStudents = group.MaxStudents,
        CurrentStudents = currentStudents,
        Status = group.Status,
        CreatedAt = group.CreatedAt
    };

    public static GroupDetailDto ToDetailDto(Group group, int currentStudents = 0) => new()
    {
        Id = group.Id,
        Name = group.Name,
        Code = group.Code,
        CourseId = group.CourseId,
        CourseName = group.Course?.Name ?? string.Empty,
        ResponsibleTeacherId = group.ResponsibleTeacherId,
        ResponsibleTeacherName = group.ResponsibleTeacher?.User?.FullName ?? string.Empty,
        DefaultTeacherId = group.DefaultTeacherId,
        DefaultTeacherName = group.DefaultTeacher?.User?.FullName,
        DefaultRoomId = group.DefaultRoomId,
        DefaultRoomName = group.DefaultRoom?.Name,
        GradingSystemId = group.GradingSystemId,
        GradingSystemName = group.GradingSystem?.Name,
        StartDate = group.StartDate,
        EndDate = group.EndDate,
        MaxStudents = group.MaxStudents,
        CurrentStudents = currentStudents,
        Status = group.Status,
        Notes = group.Notes,
        CreatedAt = group.CreatedAt,
        UpdatedAt = group.UpdatedAt
    };
}
