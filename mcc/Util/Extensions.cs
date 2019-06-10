using System.Text.RegularExpressions;

namespace mcc.Util
{
    public static class Extensions
    {
        public static string ToSafeString(this string str, string replacement = "_")
        {
            return Regex.Replace(str, @"[\W]", replacement);
        }
    }
}
