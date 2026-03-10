using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MilkTea.Shared.Utils;
// AppConfig removed - use IConfiguration directly

namespace MilkTea.Infrastructure.BuildingBlocks.Database.MySQL
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
                var dbConfig = configToUse.GetSection("DatabaseConfig");
                if (!dbConfig.Exists())
                {
                    LogHelper.Write(vCallFrom + ":\nDatabaseConfig section not found in appsettings.json");
                    return "";
                }

                string? vServer = dbConfig["Server"];
                string? vPort = dbConfig["Port"];
                string? vUsername = dbConfig["Username"];
                string? vPassword = dbConfig["Password"];
                string? vDatabase = dbConfig["Database"];
                if (string.IsNullOrEmpty(vServer) || string.IsNullOrEmpty(vUsername) ||
                    string.IsNullOrEmpty(vPassword) || string.IsNullOrEmpty(vDatabase) || string.IsNullOrEmpty(vPort))
                {
                    LogHelper.Write(vCallFrom + ":\nDatabaseConfig section is incomplete. Please provide Server, Port, Username, Password, and Database.");
                    return "";
                }

                //// RSA decryption: Attempt to decrypt each value; if decryption fails, use the original value
                //// N?u gi?i mã thành công, dùng giá tr? dã gi?i mã; n?u không, dùng giá tr? g?c
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
                //    // If decryption fails, the value might not be encrypted, use it directly
                //}

                if (string.IsNullOrEmpty(vServer) || string.IsNullOrEmpty(vUsername) ||
                    string.IsNullOrEmpty(vPassword) || string.IsNullOrEmpty(vDatabase) || string.IsNullOrEmpty(vPort))
                {
                    LogHelper.Write(vCallFrom + ":\nFailed to decrypt DatabaseConfig or values are empty after decryption.");
                    return "";
                }

                return "Server=" + vServer +
                       ";Database=" + vDatabase +
                       ";Port=" + vPort +
                       ";User Id=" + vUsername +
                       ";Password=" + vPassword;
                //";SslMode=None;";
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
