using Application.Abstractions;
using Domain.Grading;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.GradingSystems.UseCases.SeedGradingSystems;

// Seed данные — создать системы оценок по умолчанию
public class SeedGradingSystemsHandler(IDataContext db)
{
    public async Task HandleAsync(CancellationToken ct = default)
    {
        var exists = await db.GradingSystems.AnyAsync(ct);
        if (exists) return;

        var systems = new List<GradingSystem>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "5-балльная",
                Type = GradingType.Numeric,
                MinScore = 1,
                MaxScore = 5,
                PassingScore = 3,
                IsDefault = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "100-балльная",
                Type = GradingType.Numeric,
                MinScore = 0,
                MaxScore = 100,
                PassingScore = 60,
                IsDefault = false
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Зачёт/Незачёт",
                Type = GradingType.PassFail,
                MinScore = 0,
                MaxScore = 1,
                PassingScore = 1,
                IsDefault = false
            }
        };

        foreach (var s in systems)
            db.Add(s);

        await db.SaveChangesAsync(ct);
    }
}
