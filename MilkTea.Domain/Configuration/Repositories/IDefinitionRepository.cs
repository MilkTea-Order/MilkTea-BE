using MilkTea.Domain.Configuration.Entities;

namespace MilkTea.Domain.Configuration.Repositories;

/// <summary>
/// Repository interface for definition operations.
/// </summary>
public interface IDefinitionRepository
{
    /// <summary>
    /// Gets a definition by code.
    /// </summary>
    Task<Definition?> GetByCodeAsync(string code);

    /// <summary>
    /// Gets definitions by group ID.
    /// </summary>
    Task<List<Definition>> GetByGroupIdAsync(int groupId);

    /// <summary>
    /// Gets all definition groups.
    /// </summary>
    Task<List<DefinitionGroup>> GetAllGroupsAsync();

    /// <summary>
    /// Gets code prefix for bill.
    /// </summary>
    Task<Definition?> GetCodePrefixBill();
}
