using MilkTea.Domain.Catalog.Enums;
using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Catalog.Entities.Price;

public sealed class PriceListEntity : Aggregate<int>
{
    public string Name { get; private set; } = null!;
    public string? Code { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime StopDate { get; private set; }
    public int CurrencyID { get; private set; }
    public PriceListStatus Status { get; private set; }

    // Navigations
    public CurrencyEntity? Currency { get; private set; }
    private readonly List<PriceListDetailEntity> _vDetails = new();
    public IReadOnlyList<PriceListDetailEntity> Details => _vDetails.AsReadOnly();

    // For EF Core
    private PriceListEntity() { }

    public static PriceListEntity Create(
        string name,
        DateTime startDate,
        DateTime stopDate,
        int currencyId,
        int createdBy,
        string? code = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(currencyId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        if (stopDate <= startDate)
            throw new ArgumentException("StopDate must be after StartDate.");

        var now = DateTime.UtcNow;

        return new PriceListEntity
        {
            Name = name,
            Code = code,
            StartDate = startDate,
            StopDate = stopDate,
            CurrencyID = currencyId,
            Status = PriceListStatus.Draft,
            CreatedBy = createdBy,
            CreatedDate = now
        };
    }

    public bool IsActive() => Status == PriceListStatus.Active &&
                              DateTime.UtcNow >= StartDate &&
                              DateTime.UtcNow <= StopDate;

    public void Activate(int activatedBy)
    {
        if (Status == PriceListStatus.Active)
            throw new InvalidOperationException("PriceList is already active.");

        if (DateTime.UtcNow < StartDate)
            throw new InvalidOperationException("Cannot activate price list before start date.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(activatedBy);

        Status = PriceListStatus.Active;
        Touch(activatedBy);
    }

    public void Deactivate(int deactivatedBy)
    {
        if (Status == PriceListStatus.Active)
            throw new InvalidOperationException("PriceList is already inactive.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(deactivatedBy);

        Status = PriceListStatus.Draft;
        Touch(deactivatedBy);
    }

    public void Expire(int expiredBy)
    {
        if (Status == PriceListStatus.Expired)
            throw new InvalidOperationException("PriceList is already expired.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(expiredBy);

        Status = PriceListStatus.Expired;
        Touch(expiredBy);
    }

    public PriceListDetailEntity AddDetail(int menuId, int sizeId, decimal price, int createdBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(menuId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sizeId);
        ArgumentOutOfRangeException.ThrowIfNegative(price);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        var detail = PriceListDetailEntity.Create(Id, menuId, sizeId, price, createdBy);
        _vDetails.Add(detail);
        Touch(createdBy);
        return detail;
    }

    public void RemoveDetail(int detailId, int removedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(removedBy);

        var detail = _vDetails.FirstOrDefault(d => d.Id == detailId);
        if (detail is null)
            throw new InvalidOperationException("PriceListDetail not found.");

        _vDetails.Remove(detail);
        Touch(removedBy);
    }

    public void UpdateInfo(string name, string? code, int updatedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        Name = name;
        Code = code;
        Touch(updatedBy);
    }

    public void UpdateDates(DateTime startDate, DateTime stopDate, int updatedBy)
    {
        if (stopDate <= startDate)
            throw new ArgumentException("StopDate must be after StartDate.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        StartDate = startDate;
        StopDate = stopDate;
        Touch(updatedBy);
    }

    private void Touch(int updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }
}
