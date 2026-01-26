using MilkTea.Domain.SharedKernel.Abstractions;
using MilkTea.Domain.Catalog.Enums;

namespace MilkTea.Domain.Catalog.Entities;

/// <summary>
/// Dinner table entity (Aggregate Root).
/// </summary>
public sealed class DinnerTable : Aggregate<int>
{
    public string? Code { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Position { get; private set; }
    public int NumberOfSeats { get; private set; }
    public int? Longs { get; private set; }
    public int? Width { get; private set; }
    public int? Height { get; private set; }
    public byte[]? EmptyPicture { get; private set; }
    public byte[]? UsingPicture { get; private set; }
    
    /// <summary>
    /// Table status. Maps to StatusOfDinnerTableID column.
    /// </summary>
    public DinnerTableStatus Status { get; private set; }
    
    public string? Note { get; private set; }

    // For EF Core
    private DinnerTable() { }

    public static DinnerTable Create(
        string name,
        int numberOfSeats,
        int createdBy,
        string? code = null,
        string? position = null,
        int? longs = null,
        int? width = null,
        int? height = null,
        string? note = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(numberOfSeats);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        var now = DateTime.UtcNow;

        return new DinnerTable
        {
            Code = code,
            Name = name,
            Position = position,
            NumberOfSeats = numberOfSeats,
            Longs = longs,
            Width = width,
            Height = height,
            Status = DinnerTableStatus.Empty,
            Note = note,
            CreatedBy = createdBy,
            CreatedDate = now
        };
    }

    public bool IsEmpty => Status == DinnerTableStatus.Empty;
    public bool IsInUse => Status == DinnerTableStatus.InUse;
    public bool IsReserved => Status == DinnerTableStatus.Reserved;

    public void MarkAsEmpty(int updatedBy)
    {
        if (Status == DinnerTableStatus.Empty)
            throw new InvalidOperationException("Table is already empty.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        Status = DinnerTableStatus.Empty;
        Touch(updatedBy);
    }

    public void MarkAsInUse(int updatedBy)
    {
        if (Status == DinnerTableStatus.InUse)
            throw new InvalidOperationException("Table is already in use.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        Status = DinnerTableStatus.InUse;
        Touch(updatedBy);
    }

    public void MarkAsReserved(int updatedBy)
    {
        if (Status == DinnerTableStatus.Reserved)
            throw new InvalidOperationException("Table is already reserved.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        Status = DinnerTableStatus.Reserved;
        Touch(updatedBy);
    }

    public void UpdateInfo(
        string name,
        string? code = null,
        string? position = null,
        int? numberOfSeats = null,
        int? longs = null,
        int? width = null,
        int? height = null,
        string? note = null,
        int updatedBy = 0)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        Name = name;
        Code = code;
        Position = position;
        if (numberOfSeats.HasValue)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(numberOfSeats.Value);
            NumberOfSeats = numberOfSeats.Value;
        }
        Longs = longs;
        Width = width;
        Height = height;
        Note = note;
        Touch(updatedBy);
    }

    public void UpdatePictures(byte[]? emptyPicture, byte[]? usingPicture, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        EmptyPicture = emptyPicture;
        UsingPicture = usingPicture;
        Touch(updatedBy);
    }

    private void Touch(int updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }
}
