using MilkTea.Domain.Users.Entities;

namespace MilkTea.Domain.Users.Repositories;

/// <summary>
/// Repository interface for Employee aggregate operations.
/// Provides query methods only. Write operations are handled through UnitOfWork pattern.
/// Following DDD principles, repositories are read-only and persistence is managed by UnitOfWork.
/// </summary>
public interface IEmployeeRepository
{
    /// <summary>
    /// Gets an employee by their unique identifier (read-only, not tracked).
    /// </summary>
    /// <param name="id">The employee ID to search for.</param>
    /// <returns>The employee if found, otherwise null.</returns>
    Task<Employee?> GetByIdAsync(int id);

    /// <summary>
    /// Gets an employee by their unique identifier with change tracking enabled.
    /// Use this method when you need to update the entity.
    /// </summary>
    /// <param name="id">The employee ID to search for.</param>
    /// <returns>The tracked employee if found, otherwise null.</returns>
    Task<Employee?> GetByIdForUpdateAsync(int id);

    /// <summary>
    /// Gets all employees from the database (read-only, not tracked).
    /// </summary>
    /// <returns>A list of all employees.</returns>
    Task<List<Employee>> GetAllAsync();

    /// <summary>
    /// Checks if an email address already exists for another employee.
    /// Used for validation to ensure email uniqueness.
    /// </summary>
    /// <param name="email">The email address to check.</param>
    /// <param name="excludeEmployeeId">The employee ID to exclude from the check (typically the current employee being updated).</param>
    /// <returns>True if the email exists for another employee, otherwise false.</returns>
    Task<bool> IsEmailExistAsync(string email, int excludeEmployeeId);

    /// <summary>
    /// Checks if a cell phone number already exists for another employee.
    /// Used for validation to ensure phone number uniqueness.
    /// </summary>
    /// <param name="cellPhone">The cell phone number to check.</param>
    /// <param name="excludeEmployeeId">The employee ID to exclude from the check (typically the current employee being updated).</param>
    /// <returns>True if the cell phone exists for another employee, otherwise false.</returns>
    Task<bool> IsCellPhoneExistAsync(string cellPhone, int excludeEmployeeId);
}
