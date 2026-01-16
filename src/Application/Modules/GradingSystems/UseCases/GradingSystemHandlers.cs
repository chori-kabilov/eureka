using Application.Abstractions;
using Domain.Grading;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.GradingSystems.UseCases;

// Получить системы оценок
public class ListGradingSystemsHandler
{
    private readonly IDataContext _db;

    public ListGradingSystemsHandler(IDataContext db) => _db = db;

    public async Task<List<GradingSystemDto>> HandleAsync(CancellationToken ct = default)
    {
        var systems = await _db.GradingSystems.ToListAsync(ct);
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

// Создать систему оценок
public class CreateGradingSystemRequest
{
    public string Name { get; set; } = string.Empty;
    public GradingType Type { get; set; } = GradingType.Numeric;
    public decimal MinScore { get; set; } = 1;
    public decimal MaxScore { get; set; } = 5;
    public decimal PassingScore { get; set; } = 3;
    public bool IsDefault { get; set; }
}

public class CreateGradingSystemHandler
{
    private readonly IDataContext _db;

    public CreateGradingSystemHandler(IDataContext db) => _db = db;

    public async Task<GradingSystemDto> HandleAsync(CreateGradingSystemRequest request, CancellationToken ct = default)
    {
        // Если устанавливаем как default, снимаем флаг с остальных
        if (request.IsDefault)
        {
            var existing = await _db.GradingSystems.Where(g => g.IsDefault).ToListAsync(ct);
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

        _db.Add(system);
        await _db.SaveChangesAsync(ct);

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

// Seed данные — создать системы оценок по умолчанию
public class SeedGradingSystemsHandler
{
    private readonly IDataContext _db;

    public SeedGradingSystemsHandler(IDataContext db) => _db = db;

    public async Task HandleAsync(CancellationToken ct = default)
    {
        var exists = await _db.GradingSystems.AnyAsync(ct);
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
            _db.Add(s);

        await _db.SaveChangesAsync(ct);
    }
}

// DTO
public class GradingSystemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public GradingType Type { get; set; }
    public decimal MinScore { get; set; }
    public decimal MaxScore { get; set; }
    public decimal PassingScore { get; set; }
    public bool IsDefault { get; set; }
}
