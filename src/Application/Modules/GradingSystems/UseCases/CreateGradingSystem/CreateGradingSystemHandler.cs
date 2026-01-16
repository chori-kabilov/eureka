using Application.Abstractions;
using Application.Modules.GradingSystems.Dtos;
using Domain.Grading;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.GradingSystems.UseCases.CreateGradingSystem;

// Создать систему оценок
public class CreateGradingSystemHandler(IDataContext db)
{
    public async Task<GradingSystemDto> HandleAsync(CreateGradingSystemRequest request, CancellationToken ct = default)
    {
        // Если устанавливаем как default, снимаем флаг с остальных
        if (request.IsDefault)
        {
            var existing = await db.GradingSystems.Where(g => g.IsDefault).ToListAsync(ct);
            foreach (var gs in existing)
                gs.IsDefault = false;
        }

        var system = new GradingSystem
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Type = request.Type,
            MinScore = request.MinScore,
            MaxScore = request.MaxScore,
            PassingScore = request.PassingScore,
            IsDefault = request.IsDefault
        };

        db.Add(system);
        await db.SaveChangesAsync(ct);

        return new GradingSystemDto
        {
            Id = system.Id,
            Name = system.Name,
            Type = system.Type,
            MinScore = system.MinScore,
            MaxScore = system.MaxScore,
            PassingScore = system.PassingScore,
            IsDefault = system.IsDefault
        };
    }
}
