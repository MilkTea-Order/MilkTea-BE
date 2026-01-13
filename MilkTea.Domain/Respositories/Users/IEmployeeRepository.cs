using MilkTea.Domain.Entities.Users;

namespace MilkTea.Domain.Respositories.Users
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetByIdAsync(int employeeId);
        void Update(Employee employee);

        Task<bool> IsEmailExistAsync(string email, int excludeEmployeeId);
        Task<bool> IsCellPhoneExistAsync(string cellPhone, int excludeEmployeeId);
    }
}
