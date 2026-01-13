namespace MilkTea.Domain.Respositories.Users
{
    public interface IPermissionRepository
    {
        Task<List<Dictionary<string, object?>>> GetPermissionsByUserId(int userId);
    }
}

