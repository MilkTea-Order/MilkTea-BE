using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MilkTea.Shared.Utils;
using static MilkTea.Domain.Configurations.App.AppConfig;

namespace MilkTea.Infrastructure.Database.MySQL
{
    public class MySQLProvider(IConfiguration configuration) : IDBProvider
    {
        public readonly IConfiguration _vConfiguration = configuration;

        public string GetConnectionString(string vCallBy, IConfiguration? configuration = null)
        {
            string vCallFrom = vCallBy + " -> " +
                               // Declaring Type (class, struct, ...) of this method
                               System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType + " -> " +
                               // Name of this method
                               System.Reflection.MethodBase.GetCurrentMethod()?.Name;
            try
            {
                // Use the provided configuration, or fall back to the static _configuration
                IConfiguration? configToUse = configuration ?? _vConfiguration;

                if (configToUse == null)
                {
                    LogHelper.Write(vCallFrom + ":\nIConfiguration is required. Please call DataProvider.SetConfiguration() in Program.cs or pass IConfiguration parameter.");
                    return "";
                }

                // Get DatabaseConfig section from appsettings.json
                var dbConfig = configToUse.GetSection(DatabaseConfig.Section);
                if (!dbConfig.Exists())
                {
                    LogHelper.Write(vCallFrom + ":\nDatabaseConfig section not found in appsettings.json");
                    return "";
                }

                string? vServer = dbConfig[DatabaseConfig.Server];
                string? vPort = dbConfig[DatabaseConfig.Port];
                string? vUsername = dbConfig[DatabaseConfig.Username];
                string? vPassword = dbConfig[DatabaseConfig.Password];
                string? vDatabase = dbConfig[DatabaseConfig.Database];
                if (string.IsNullOrEmpty(vServer) || string.IsNullOrEmpty(vUsername) ||
                    string.IsNullOrEmpty(vPassword) || string.IsNullOrEmpty(vDatabase) || string.IsNullOrEmpty(vPort))
                {
                    LogHelper.Write(vCallFrom + ":\nDatabaseConfig section is incomplete. Please provide Server, Port, Username, Password, and Database.");
                    return "";
                }

                //// Thử giải mã RSA (nếu là chuỗi đã mã hóa, sẽ có format đặc biệt)
                //// Nếu giải mã thành công, dùng giá trị đã giải mã; nếu không, dùng giá trị gốc
                //try
                //{
                //    vServer = Definitions.RSA.Decrypt(vCallFrom, Definitions.Parameters.RSA_PrivateKey, vServer);
                //    vPort = Definitions.RSA.Decrypt(vCallFrom, Definitions.Parameters.RSA_PrivateKey, vPort);
                //    vUsername = Definitions.RSA.Decrypt(vCallFrom, Definitions.Parameters.RSA_PrivateKey, vUsername);
                //    vPassword = Definitions.RSA.Decrypt(vCallFrom, Definitions.Parameters.RSA_PrivateKey, vPassword);
                //    vDatabase = Definitions.RSA.Decrypt(vCallFrom, Definitions.Parameters.RSA_PrivateKey, vDatabase);
                //}
                //catch
                //{
                //    // Nếu giải mã thất bại, có thể giá trị chưa được mã hóa, dùng trực tiếp
                //}

                if (string.IsNullOrEmpty(vServer) || string.IsNullOrEmpty(vUsername) ||
                    string.IsNullOrEmpty(vPassword) || string.IsNullOrEmpty(vDatabase) || string.IsNullOrEmpty(vPort))
                {
                    LogHelper.Write(vCallFrom + ":\nFailed to decrypt DatabaseConfig or values are empty after decryption.");
                    return "";
                }

                return "Server='" + vServer + "';AllowLoadLocalInfile=true;Database=" + vDatabase + ";port=" + vPort + ";User Id=" + vUsername + ";password=" + vPassword;
            }
            catch (Exception ex)
            {
                LogHelper.Write($"{vCallFrom}:\n{ex.Message}\n{ex.StackTrace}");
                return "";
            }
        }

        public DbContextOptionsBuilder ConfigureDbContext(DbContextOptionsBuilder options, string vCallBy)
        {
            var connectionString = GetConnectionString(vCallBy);
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Database connection string is not configured. " +
                    "Please set 'DatabaseConfig' section in appsettings.json with Server, Port, Username, Password, and Database.");
            }
            return options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
