// See https://aka.ms/new-console-template for more information
using MilkTea.Shared.Utils.Hash;
using MilkTea.Shared.Domain.Constants;


Console.WriteLine("IP: " +  RSAHelper.Encrypt("test", Key.RSA_PublicKey, "db"));
Console.WriteLine("PORT: " +  RSAHelper.Encrypt("test", Key.RSA_PublicKey, "3306"));
Console.WriteLine("User: " + RSAHelper.Encrypt("test", Key.RSA_PublicKey, "user"));
Console.WriteLine("Password: " +  RSAHelper.Encrypt("test", Key.RSA_PublicKey, "123456"));
Console.WriteLine("Database: " + RSAHelper.Encrypt("test", Key.RSA_PublicKey, "MilkTea"));



