using System.Reflection;

namespace MilkTea.Domain.Entities
{
    public class BaseModel
    {
        // Converts the object properties to a dictionary
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
