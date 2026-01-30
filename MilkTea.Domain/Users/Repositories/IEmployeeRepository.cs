using MilkTea.Domain.Users.Entities;

namespace MilkTea.Domain.Users.Repositories;

/// <summary>
/// Repository interface for Employee aggregate operations.
/// </summary>
public interface IEmployeeRepository
{
    /// <summary>
    /// Gets an employee by their unique identifier (read-only, not tracked).
    /// </summary>
    /// <param name="id">The employee ID to search for.</param>
    /// <returns>The employee if found, otherwise null.</returns>
    Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets an employee by their unique identifier with change tracking enabled.
    /// </summary>
    /// <param name="id">The employee ID to search for.</param>
    /// <returns>The tracked employee if found, otherwise null.</returns>
    Task<Employee?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all employees from the database (read-only, not tracked).
    /// </summary>
    /// <returns>A list of all employees.</returns>
    Task<List<Employee>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets an employee by ID including its <see cref="Gender"/> and <see cref="Position"/> navigation properties.
    /// </summary>
    /// <param name="id">The employee ID to search for.</param>
    /// <returns>The employee with related entities if found, otherwise null.</returns>
    Task<Employee?> GetByIdWithGenderAndPositionAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Checks if an email address already exists for another employee.
    /// Used for validation to ensure email uniqueness.
    /// </summary>
    /// <param name="email">The email address to check.</param>
    /// <param name="excludeEmployeeId">The employee ID to exclude from the check (typically the current employee being updated).</param>
    /// <returns>True if the email exists for another employee, otherwise false.</returns>
    Task<bool> IsEmailExistAsync(string email, int excludeEmployeeId, CancellationToken cancellationToken);

    /// <summary>
    /// Checks if a cell phone number already exists for another employee.
    /// Used for validation to ensure phone number uniqueness.
    /// </summary>
    /// <param name="cellPhone">The cell phone number to check.</param>
    /// <param name="excludeEmployeeId">The employee ID to exclude from the check (typically the current employee being updated).</param>
    /// <returns>True if the cell phone exists for another employee, otherwise false.</returns>
    Task<bool> IsCellPhoneExistAsync(string cellPhone, int excludeEmployeeId, CancellationToken cancellationToken);
}
