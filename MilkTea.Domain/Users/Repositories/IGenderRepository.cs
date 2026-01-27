using MilkTea.Domain.Users.Entities;

namespace MilkTea.Domain.Users.Repositories;

/// <summary>
/// Repository interface for Gender lookup entity operations.
/// Gender is a reference data entity used for employee profile information.
/// </summary>
public interface IGenderRepository
{
    /// <summary>
    /// Gets a gender by its unique identifier.
    /// </summary>
    /// <param name="id">The gender ID to search for.</param>
    /// <returns>The gender if found, otherwise null.</returns>
    Task<Gender?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all available genders from the database.
    /// </summary>
    /// <returns>A list of all genders.</returns>
    Task<List<Gender>> GetAllAsync();

    /// <summary>
    /// Checks if a gender with the specified ID exists in the database.
    /// Used for validation before creating or updating employee records.
    /// </summary>
    /// <param name="genderId">The gender ID to check.</param>
    /// <returns>True if the gender exists, otherwise false.</returns>
    Task<bool> ExistsGenderAsync(int genderId);
}
