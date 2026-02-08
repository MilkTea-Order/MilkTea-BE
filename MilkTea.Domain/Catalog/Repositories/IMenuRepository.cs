using MilkTea.Domain.Catalog.Entities.Menu;

namespace MilkTea.Domain.Catalog.Repositories;

/// <summary>
/// Repository interface for menu-related data operations.
/// </summary>
public interface IMenuRepository
{
    /// <summary>
    /// Gets all menu groups.
    /// </summary>
    Task<List<MenuGroupEntity>> GetAllMenuGroupsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets all active menu groups.
    /// </summary>
    Task<List<MenuGroupEntity>> GetAlllActiveMenuGroupsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets group menu relations by group ID.
    /// </summary>
    Task<MenuGroupEntity?> GetByIdWithMenuAsync(int groupId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets group menu relations by group ID and menu status.
    /// </summary>
    Task<MenuGroupEntity?> GetByIdWithMenuAsync(int groupId, int? menuStatusId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets group menu relations by menuID
    /// </summary>
    Task<MenuGroupEntity?> GetByMenuIdWithRelationshipsAsync(int menuId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets menu groups by status.
    /// </summary>
    Task<List<MenuGroupEntity>> GetByStatusWithMenuAsync(int? statusId, int? itemStatusId, CancellationToken cancellationToken);

    /// <summary>
    /// Check menu and size is status active (can pay)
    /// </summary>
    /// <param name="menuId"></param>
    /// <param name="sizeId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>menu and size are active(can pay)</returns>
    Task<bool> isActiceMenuAndSize(int menuId, int sizeId, CancellationToken cancellationToken);
}

