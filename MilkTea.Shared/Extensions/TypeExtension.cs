namespace MilkTea.Shared.Extensions
{
    public static class TypeExtension
    {
        public static bool IsSimpleType(this Type type)
        {
            var t = Nullable.GetUnderlyingType(type) ?? type;

            return t.IsPrimitive
                 || t.IsEnum
                 || t == typeof(string)
                 || t == typeof(decimal)
                 || t == typeof(DateTime)
                 || t == typeof(Guid);
        }
    }
}


