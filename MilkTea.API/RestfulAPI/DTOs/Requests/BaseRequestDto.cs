using System.Reflection;

namespace MilkTea.API.RestfulAPI.DTOs.Requests
{
    public abstract class BaseRequestDto
    {
        public Dictionary<string, object?> ToDictionary()
        {
            return GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => prop.GetValue(this) != null)
                .ToDictionary(
                    prop => prop.Name,
                    prop => prop.GetValue(this, null)
                );
        }
    }
}
