using MilkTea.Domain.Users.Entities;

namespace MilkTea.Domain.Users.Repositories;

/// <summary>
/// Repository interface for gender operations.
/// </summary>
public interface IGenderRepository
{
    /// <summary>
    /// Gets a gender by ID.
    /// </summary>
    Task<Gender?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all genders.
    /// </summary>
    Task<List<Gender>> GetAllAsync();

    /// <summary>
    /// Checks if a gender exists.
    /// </summary>
    Task<bool> ExistsGenderAsync(int genderId);
}
