using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Catalog.Entities;
using MilkTea.Domain.Catalog.Enums;
using MilkTea.Domain.Catalog.Repositories;
using MilkTea.Domain.SharedKernel.Enums;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Catalog;

/// <summary>
/// Repository implementation for menu-related data operations.
/// </summary>
public sealed class MenuRepository(AppDbContext context) : IMenuRepository
{
    private readonly AppDbContext _vContext = context;
    /// <inheritdoc/>
    public async Task<List<MenuGroup>> GetAllMenuGroupsAsync(CancellationToken cancellationToken)
    {
        return await _vContext.MenuGroups
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    /// <inheritdoc/>
    public async Task<List<MenuGroup>> GetAlllActiveMenuGroupsAsync(CancellationToken cancellationToken)
    {
        return await _vContext.MenuGroups
            .AsNoTracking()
            .Where(mg => mg.Status == CommonStatus.Active)
            .ToListAsync(cancellationToken);
    }

    #region Relationship
    /// <inheritdoc/>
    public async Task<MenuGroup?> GetByIdWithMenuAsync(int groupId, CancellationToken cancellationToken)
    {
        return await _vContext.MenuGroups
                        .AsNoTracking()
                        .Include(mg => mg.Menus)
                        .Where(mg => mg.Id == groupId)
                        .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<MenuGroup?> GetByIdWithMenuAsync(
                                                    int groupId,
                                                    int? menuStatusId,
                                                    CancellationToken cancellationToken)
    {
        var query = _vContext.MenuGroups
                                .AsNoTracking()
                                .Where(mg => mg.Id == groupId);

        if (menuStatusId.HasValue)
        {
            query = query
                .Include(mg => mg.Menus.Where(m => m.Status == (MenuStatus)menuStatusId.Value))
                .Where(mg => mg.Menus.Any(m => m.Status == (MenuStatus)menuStatusId.Value));
        }
        else
        {
            query = query.Include(mg => mg.Menus);
        }
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<MenuGroup>> GetByStatusWithMenuAsync(
    int? statusId, int? itemStatusId, CancellationToken cancellationToken)
    {
        var query = _vContext.MenuGroups
            .AsNoTracking()
            .AsSplitQuery();

        if (statusId.HasValue)
        {
            var status = (CommonStatus)statusId.Value;
            query = query.Where(mg => mg.Status == status);
        }

        if (itemStatusId.HasValue)
        {
            var menuStatus = (MenuStatus)itemStatusId.Value;

            query = query
                .Where(mg => mg.Menus.Any(m => m.Status == menuStatus))
                .Include(mg => mg.Menus.Where(m => m.Status == menuStatus));
        }
        else
        {
            query = query.Include(mg => mg.Menus);
        }

        return await query.ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<MenuGroup?> GetByMenuIdWithRelationshipsAsync(int menuId, CancellationToken cancellationToken)
    {
        var groupId = await _vContext.Menus
                                .AsNoTracking()
                                .Where(m => m.Id == menuId)
                                .Select(m => m.MenuGroupID)
                                .FirstOrDefaultAsync(cancellationToken);

        if (groupId <= 0) return null;

        return await _vContext.MenuGroups
            .AsNoTracking()
            .AsSplitQuery()
            .Include(g => g.Menus.Where(m => m.Id == menuId))
            .ThenInclude(m => m.MenuSizes)
            .FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);
    }
    #endregion Relationship 

}

