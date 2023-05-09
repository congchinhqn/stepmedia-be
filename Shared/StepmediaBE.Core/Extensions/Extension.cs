

namespace Metatrade.Core.Extensions
{
    public static class Extension
    {
        public static object GetValueOrDefault<T>(this T? val, object defaultVal) where T : struct
        {
            if (val != null)
                return val.Value;

            return defaultVal;
        }

        public static object GetValueOrDefault(this string val, object defaultVal)
        {
            if (string.IsNullOrWhiteSpace(val))
                return defaultVal;
            return val.Trim();
        }
    }
}
