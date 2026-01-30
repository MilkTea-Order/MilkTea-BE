using MilkTea.Domain.Catalog.Enums;
using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Catalog.Entities;

public sealed class TableEntity : Aggregate<int>
{
    public string? Code { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Position { get; private set; }
    public int NumberOfSeats { get; private set; }
    public int? Longs { get; private set; }
    public int? Width { get; private set; }
    public int? Height { get; private set; }
    public byte[] EmptyPicture { get; private set; } = null!;
    public byte[] UsingPicture { get; private set; } = null!;
    public TableStatus Status { get; private set; }
    public string? Note { get; private set; }

    // For EF Core
    private TableEntity()
    {
    }

    public static TableEntity Create(
        string name,
        int numberOfSeats,
        int createdBy,
        byte[] emptyPicture,
        byte[] ussingPicture,
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
        ArgumentNullException.ThrowIfNull(emptyPicture);
        ArgumentNullException.ThrowIfNull(ussingPicture);

        var now = DateTime.UtcNow;

        return new TableEntity
        {
            Code = code,
            Name = name,
            Position = position,
            NumberOfSeats = numberOfSeats,
            Longs = longs,
            Width = width,
            Height = height,
            Status = TableStatus.InUsing,
            Note = note,
            EmptyPicture = emptyPicture,
            UsingPicture = ussingPicture,
            CreatedBy = createdBy,
            CreatedDate = now
        };
    }

    public bool IsInRepairing => Status == TableStatus.InRepairing;
    public bool IsInUsing => Status == TableStatus.InUsing;
    public bool IsOutOfService => Status == TableStatus.IsOutOfService;

    public void MarkAsReparing(int updatedBy)
    {
        if (Status == TableStatus.InRepairing)
            throw new InvalidOperationException("Table is already empty.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        Status = TableStatus.InRepairing;
        Touch(updatedBy);
    }

    public void MarkAsInUsing(int updatedBy)
    {
        if (Status == TableStatus.InUsing)
            throw new InvalidOperationException("Table is already in use.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        Status = TableStatus.InUsing;
        Touch(updatedBy);
    }

    public void MarkAsOutOfService(int updatedBy)
    {
        if (Status == TableStatus.IsOutOfService)
            throw new InvalidOperationException("Table is already out of service.");
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        Status = TableStatus.IsOutOfService;
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

    public void UpdatePictures(byte[] emptyPicture, byte[] usingPicture, int updatedBy)
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
