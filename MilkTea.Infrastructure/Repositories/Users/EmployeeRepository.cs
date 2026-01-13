using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Entities.Users;
using MilkTea.Domain.Respositories.Users;
using MilkTea.Infrastructure.Persistence;

namespace MilkTea.Infrastructure.Repositories.Users
{
    public class EmployeeRepository(AppDbContext context) : IEmployeeRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Employee?> GetByIdAsync(int employeeId)
        {
            return await _context.Employees.FirstOrDefaultAsync(x => x.ID == employeeId);
        }

        public void Update(Employee employee)
        {
            _context.Employees.Update(employee);
        }

        public async Task<bool> IsEmailExistAsync(string email, int excludeEmployeeId)
        {
            return await _context.Employees
                .AnyAsync(e => e.Email == email && e.ID != excludeEmployeeId);
        }

        public async Task<bool> IsCellPhoneExistAsync(string cellPhone, int excludeEmployeeId)
        {
            return await _context.Employees
                .AnyAsync(e => e.CellPhone == cellPhone && e.ID != excludeEmployeeId);
        }
    }
}
