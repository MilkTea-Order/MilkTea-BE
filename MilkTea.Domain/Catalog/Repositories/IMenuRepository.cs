using MilkTea.Domain.Catalog.Entities;

namespace MilkTea.Domain.Catalog.Repositories;

/// <summary>
/// Repository interface for menu-related data operations.
/// </summary>
public interface IMenuRepository
{
    /// <summary>
    /// Gets all menu groups.
    /// </summary>
    Task<List<MenuGroup>> GetAllMenuGroupsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets all active menu groups.
    /// </summary>
    Task<List<MenuGroup>> GetAlllActiveMenuGroupsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets group menu relations by group ID.
    /// </summary>
    Task<MenuGroup?> GetByIdWithMenuAsync(int groupId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets group menu relations by group ID and menu status.
    /// </summary>
    Task<MenuGroup?> GetByIdWithMenuAsync(int groupId, int? menuStatusId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets group menu relations by menuID
    /// </summary>
    Task<MenuGroup?> GetByMenuIdWithRelationshipsAsync(int menuId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets menu groups by status.
    /// </summary>
    Task<List<MenuGroup>> GetByStatusWithMenuAsync(int? statusId, int? itemStatusId, CancellationToken cancellationToken);


}

