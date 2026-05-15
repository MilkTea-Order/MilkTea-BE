using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Catalog.Abstractions.Queries;
using MilkTea.Application.Features.Catalog.Models.Dtos;
using MilkTea.Application.Features.Catalog.Models.Dtos.Currency;
using MilkTea.Application.Features.Catalog.Models.Dtos.Menu;
using MilkTea.Application.Features.Catalog.Models.Dtos.Price;
using MilkTea.Application.Features.Catalog.Models.Dtos.Size;
using MilkTea.Domain.Catalog.Menu.Enums;
using MilkTea.Domain.Catalog.Price.Enums;
using MilkTea.Domain.Common.Enums;
using MilkTea.Infrastructure.Persistence;
using Shared.Extensions;

namespace MilkTea.Infrastructure.Features.Catalog.Queries
{
    public class CatalogQuery(AppDbContext context) : ICatalogQuery
    {
        private readonly AppDbContext _vContext = context;

        ///<inheritdoc/>
        public async Task<List<MenuDto>> GetMenusAsync(int? groupId,
            string? name,
            CommonStatus menuStatus = CommonStatus.Active,
            CommonStatus groupStatus = CommonStatus.Active,
            CancellationToken cancellationToken = default)
        {
            var hasGroup = groupId.HasValue;
            var hasName = !string.IsNullOrWhiteSpace(name);
            if (!hasGroup && !hasName) return new();

            var activePriceListId = menuStatus == CommonStatus.Active
                ? await _vContext.PriceLists.AsNoTracking()
                    .Where(x => x.Status == PriceListStatus.Active)
                    .Select(x => (int?)x.Id)
                    .FirstOrDefaultAsync(cancellationToken)
                : null;

            var rows = await _vContext.Menus.AsNoTracking()
                // Filter option group or name or both
                .Where(m =>
                    (!hasGroup || m.MenuGroupId == groupId!.Value)
                    // && (!hasName || EF.Functions.Like(
                    //     EF.Functions.Collate(m.Name, "Vietnamese_CI_AS"),
                    //     $"%{name}%")
                    && (!hasName || EF.Functions.Like(m.Name, $"%{name}%"))
                    && m.Status == menuStatus)

                // Filter group status
                .Where(m => _vContext.MenuGroups.AsNoTracking()
                    .Any(g => g.Id == m.MenuGroupId && g.Status == groupStatus))

                //Filter menu stauts
                .Where(m => menuStatus != CommonStatus.Active ||
                            _vContext.PriceListDetails.AsNoTracking()
                                .Any(p => p.MenuID == m.Id
                                          && activePriceListId != null
                                          && p.PriceListID == activePriceListId.Value))
                //Join unit information
                .Join(_vContext.Units.AsNoTracking(),
                    m => m.UnitId,
                    u => u.Id,
                    (m, u) => new { m, u })
                .Select(x => new
                {
                    x.m.Id,
                    x.m.Code,
                    x.m.Name,
                    x.m.Note,
                    x.m.AvatarPicture,
                    MenuGroupId = x.m.MenuGroupId,
                    UnitId = x.u.Id,
                    UnitName = x.u.Name,
                    StatusId = (int)x.m.Status
                })
                .OrderBy(x => x.Id)
                .ToListAsync(cancellationToken);

            return rows.Select(x => new MenuDto
            {
                MenuId = x.Id,
                MenuCode = x.Code,
                MenuName = x.Name,
                Note = x.Note,
                MenuImage = x.AvatarPicture != null
                    ? $"data:image/png;base64,{Convert.ToBase64String(x.AvatarPicture)}"
                    : null,
                MenuGroupId = x.MenuGroupId,
                UnitId = x.UnitId,
                UnitName = x.UnitName,
                Status = new StatusDto
                {
                    Id = x.StatusId,
                    Name = ((MenuStatus)x.StatusId).GetDescription()
                }
            }).ToList();
        }

        public async Task<List<MenuGroupDto>> GetGroupMenuAvailableAsync(CancellationToken cancellationToken = default)
        {
            // lấy active price list đang được dùng hiện tại
            var activePriceListId = await _vContext.PriceLists
                .AsNoTracking()
                .Where(x => x.Status == PriceListStatus.Active)
                .Select(x => (int?)x.Id)
                .FirstOrDefaultAsync(cancellationToken);

            var rows = await _vContext.MenuGroups
                .AsNoTracking()
                .Where(g => g.Status == CommonStatus.Active)
                .Select(g => new
                {
                    g.Id,
                    g.Name,
                    StatusId = (int)g.Status,
                    MenuCount = _vContext.Menus
                        .AsNoTracking()
                        .Count(m =>
                            m.MenuGroupId == g.Id
                            && m.Status == CommonStatus.Active
                            && _vContext.PriceListDetails
                                .AsNoTracking()
                                .Any(p =>
                                    p.MenuID == m.Id
                                    && activePriceListId != null
                                    && p.PriceListID == activePriceListId.Value))
                })
                .ToListAsync(cancellationToken);

            return rows.Select(x => new MenuGroupDto
            {
                MenuGroupId = x.Id,
                MenuGroupName = x.Name,
                Status = new StatusDto
                {
                    Id = x.StatusId,
                    Name = ((CommonStatus)x.StatusId).GetDescription()
                },
                Quantity = x.MenuCount
            }).ToList();
        }

        public async Task<List<SizeAndPriceDto>> GetMenuSizesAsync(int menuId, CancellationToken cancellationToken = default)
        {
            if (menuId <= 0)
                return new List<SizeAndPriceDto>();

            var activePriceInfo = await _vContext.PriceLists
                .AsNoTracking()
                .Where(p => p.Status == PriceListStatus.Active)
                .Select(p => new
                {
                    p.Id,
                    p.CurrencyID
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (activePriceInfo is null)
                return new List<SizeAndPriceDto>();

            var activePriceListId = activePriceInfo.Id;
            var currencyId = activePriceInfo.CurrencyID;

            return await _vContext.Menus
                .AsNoTracking()

                // MenuGroup
                .Join(
                    _vContext.MenuGroups.AsNoTracking(),
                    menu => menu.MenuGroupId,
                    group => group.Id,
                    (menu, group) => new
                    {
                        menu,
                        group
                    })

                // MenuSize
                .Join(
                    _vContext.MenuSizes.AsNoTracking(),
                    x => x.menu.Id,
                    menuSize => menuSize.MenuID,
                    (x, menuSize) => new
                    {
                        x.menu,
                        x.group,
                        menuSize
                    })

                // Size
                .Join(
                    _vContext.Sizes.AsNoTracking(),
                    x => x.menuSize.SizeID,
                    size => size.Id,
                    (x, size) => new
                    {
                        x.menu,
                        x.group,
                        x.menuSize,
                        size
                    })

                // PriceListDetail
                .Join(
                    _vContext.PriceListDetails.AsNoTracking(),
                    x => new
                    {
                        MenuID = x.menu.Id,
                        SizeID = x.size.Id,
                        PriceListID = activePriceListId
                    },
                    detail => new
                    {
                        detail.MenuID,
                        detail.SizeID,
                        detail.PriceListID
                    },
                    (x, detail) => new
                    {
                        x.menu,
                        x.group,
                        x.menuSize,
                        x.size,
                        detail
                    })

                // Currency
                .Join(
                    _vContext.Currencies.AsNoTracking(),
                    x => currencyId,
                    currency => currency.Id,
                    (x, currency) => new
                    {
                        x.menu,
                        x.group,
                        x.menuSize,
                        x.size,
                        x.detail,
                        currency
                    })
                .Where(x =>
                    x.menu.Id == menuId &&
                    x.menu.Status == CommonStatus.Active &&
                    x.group.Status == CommonStatus.Active)
                .OrderBy(x => x.size.RankIndex)
                .Select(x => new SizeAndPriceDto
                {
                    Size = new SizeDto
                    {
                        SizeId = x.size.Id,
                        SizeName = x.size.Name,
                        RankIndex = x.size.RankIndex
                    },

                    Price = new PriceDto
                    {
                        PriceListId = activePriceListId,
                        Price = x.detail.Price,

                        Currency = new CurrencyDto
                        {
                            Id = x.currency.Id,
                            Name = x.currency.Name,
                            Code = x.currency.Code
                        }
                    }
                })
                .ToListAsync(cancellationToken);
        }   
    }
}

