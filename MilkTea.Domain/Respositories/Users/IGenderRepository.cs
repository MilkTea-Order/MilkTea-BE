namespace MilkTea.Domain.Respositories.Users
{
    public interface IGenderRepository
    {
        Task<bool> ExistsGenderAsync(int genderId);
    }
}
