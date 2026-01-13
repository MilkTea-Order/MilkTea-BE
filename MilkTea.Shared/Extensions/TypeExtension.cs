namespace MilkTea.Shared.Extensions
{
    public static class TypeExtension
    {
        public static bool IsSimpleType(this Type type)
        {
            return type.IsPrimitive
                 || type.IsEnum
                 || type.Equals(typeof(string))
                 || type.Equals(typeof(decimal))
                 || type.Equals(typeof(DateTime))
                 || type.Equals(typeof(Guid));
        }
    }
}


