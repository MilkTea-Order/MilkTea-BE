namespace MilkTea.Shared.Extensions
{
    public static class DictionaryExtensions
    {
        public static List<Dictionary<string, object?>> ToDictList<T>(this IEnumerable<T> source, bool flatten = true)
        {
            return [.. source.Select(item => ToDict(item!, flatten))];
        }


        private static Dictionary<string, object?> ToDict(object obj, bool flatten)
        {
            var dict = new Dictionary<string, object?>();
            var props = obj.GetType().GetProperties();

            foreach (var prop in props)
            {
                if (prop.GetIndexParameters().Length > 0)
                    continue;

                var name = prop.Name;
                var value = prop.GetValue(obj);

                if (value != null
                    && typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType)
                    && prop.PropertyType != typeof(string))
                {
                    continue;
                }

                if (value != null && !prop.PropertyType.IsSimpleType())
                {
                    if (flatten)
                    {
                        // Flatten: merge các properties của navigation property vào dict chính
                        var nested = ToDict(value, flatten);
                        foreach (var kvp in nested)
                            dict[kvp.Key] = kvp.Value; // ← Đây là lý do TotalAmount bị ghi đè!
                    }
                    else
                    {
                        // Không flatten: bỏ qua navigation properties
                        continue;
                    }
                }
                else
                {
                    dict[name] = value;
                }
            }

            return dict;
        }
    }
}

//private static Dictionary<string, object?> ToDict(object obj)
//{
//    var dict = new Dictionary<string, object?>();
//    var props = obj.GetType().GetProperties();

//    foreach (var prop in props)
//    {
//        var name = prop.Name;
//        var value = prop.GetValue(obj);

//        if (value != null && !prop.PropertyType.IsSimpleType())
//        {
//            // Recursively flatten complex types
//            var nested = ToDict(value);
//            foreach (var kvp in nested)
//                dict[kvp.Key] = kvp.Value;
//        }
//        else
//        {
//            dict[name] = value;
//        }
//    }

//    return dict;
//}