using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace McFuncCompiler.Util
{
    public static class Extensions
    {
        public static string ToSafeString(this string str, string replacement = "_")
        {
            return Regex.Replace(str, @"[\W]", replacement);
        }
    }
}
