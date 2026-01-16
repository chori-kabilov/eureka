using Application.Abstractions;
using Application.Modules.GradingSystems.Dtos;
using Domain.Grading;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.GradingSystems.UseCases.ListGradingSystems;

// Получить системы оценок
public class ListGradingSystemsHandler(IDataContext db)
{
    public async Task<List<GradingSystemDto>> HandleAsync(CancellationToken ct = default)
    {
        var systems = await db.GradingSystems.ToListAsync(ct);
        return systems.Select(s => new GradingSystemDto
        {
            Id = s.Id,
            Name = s.Name,
            Type = s.Type,
            MinScore = s.MinScore,
            MaxScore = s.MaxScore,
            PassingScore = s.PassingScore,
            IsDefault = s.IsDefault
        }).ToList();
    }
}
