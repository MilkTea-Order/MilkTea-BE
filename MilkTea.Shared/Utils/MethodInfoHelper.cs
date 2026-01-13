namespace MilkTea.Shared.Utils
{
    public static class MethodInfoHelper
    {
        /// <summary>
        /// Lấy tên class và method hiện tại
        /// </summary>
        public static string GetCallFrom()
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            var frame = stackTrace.GetFrame(1);
            var method = frame?.GetMethod();

            if (method == null) return "Unknown";

            var className = method.DeclaringType?.Name ?? "Unknown";
            var methodName = method.Name;

            return $"{className} -> {methodName}";
        }

        /// <summary>
        /// Lấy callFrom với frame offset tùy chỉnh
        /// </summary>
        public static string GetCallFrom(int frameOffset)
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            var frame = stackTrace.GetFrame(frameOffset);
            var method = frame?.GetMethod();

            if (method == null) return "Unknown";

            var className = method.DeclaringType?.Name ?? "Unknown";
            var methodName = method.Name;

            return $"{className} -> {methodName}";
        }

    }
}
