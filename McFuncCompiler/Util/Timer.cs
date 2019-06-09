using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFuncCompiler.Util
{
    public static class Timer
    {
        private static Dictionary<string, DateTime> _timers = new Dictionary<string, DateTime>();

        public static void Start(string name)
        {
            _timers.Remove(name);
            _timers.Add(name, DateTime.UtcNow);
        }

        public static TimeSpan End(string name)
        {
            return DateTime.UtcNow - _timers[name];
        }
    }
}
