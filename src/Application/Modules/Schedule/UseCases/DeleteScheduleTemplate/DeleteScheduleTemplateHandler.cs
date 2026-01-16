using Application.Abstractions;
using Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Schedule.UseCases.DeleteScheduleTemplate;

// Удалить шаблон расписания
public class DeleteScheduleTemplateHandler(IDataContext db)
{
    public async Task<Result<bool>> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var template = await db.ScheduleTemplates.FirstOrDefaultAsync(t => t.Id == id, ct);
        if (template == null)
            return Result<bool>.Failure(Error.NotFound("Шаблон"));

        db.Remove(template);
        await db.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}
