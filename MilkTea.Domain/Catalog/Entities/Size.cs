using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Catalog.Entities;

/// <summary>
/// Size entity for menu items (S, M, L, etc.).
/// </summary>
public sealed class Size : Entity<int>
{
    public string Name { get; private set; } = null!;
    public int RankIndex { get; private set; }

    // For EF Core
    private Size() { }

    public static Size Create(string name, int rankIndex, int createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegative(rankIndex);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        var now = DateTime.UtcNow;

        return new Size
        {
            Name = name,
            RankIndex = rankIndex,
            CreatedBy = createdBy,
            CreatedDate = now
        };
    }

    public void UpdateName(string name, int updatedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        Name = name;
        Touch(updatedBy);
    }

    public void UpdateRankIndex(int rankIndex, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(rankIndex);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        RankIndex = rankIndex;
        Touch(updatedBy);
    }

    private void Touch(int updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }
}
