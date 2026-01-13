namespace MilkTea.Domain.Configurations.App
{
    public class AppConfig
    {
        public static class DatabaseConfig
        {
            public const string Section = "DatabaseConfig";
            public const string Server = "Server";
            public const string Provider = "Provider";
            public const string Port = "Port";
            public const string Username = "Username";
            public const string Password = "Password";
            public const string Database = "Database";
        }

        //public static class Jwt
        //{
        //    public const string Section = "JwtSettings";
        //    public const string Key = "Key";
        //    public const string Issuer = "Issuer";
        //    public const string Audience = "Audience";
        //    public const string AccessTokenExpirationMinutes = "AccessTokenExpirationMinutes";
        //    public const string RefreshTokenExpirationMinutes = "RefreshTokenExpirationMinutes";
        //}


        //public static class Redis
        //{
        //    public const string Section = "Redis";
        //    public const string Host = "Host";
        //    public const string Port = "Port";
        //}


    }
}
