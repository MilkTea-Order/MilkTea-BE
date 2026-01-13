using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MilkTea.Infrastructure.Database;
public interface IDBProvider
{
    //public string GetConnectionString(IConfiguration configuration);
    public string GetConnectionString(string vCallBy, IConfiguration? configuration = null);
    DbContextOptionsBuilder ConfigureDbContext(DbContextOptionsBuilder options, string vCallBy);
}
