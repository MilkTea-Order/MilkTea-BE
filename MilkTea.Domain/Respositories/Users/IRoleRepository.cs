namespace MilkTea.Domain.Respositories.Users
{
    public interface IRoleRepository
    {
        Task<List<string>> GetRolesByUserId(int userId);
    }
}
