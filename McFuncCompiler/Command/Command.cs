using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFuncCompiler.Command
{
    /// <summary>
    /// Represents a singular command.
    /// </summary>
    public class Command
    {
        public List<Argument> Arguments = new List<Argument>();
    }
}
