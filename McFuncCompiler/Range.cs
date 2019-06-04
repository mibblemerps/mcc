using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFuncCompiler
{
    /// <summary>
    /// Simple range.
    /// </summary>
    public sealed class Range
    {
        public int Minimum { get; set; }
        public int Maximum { get; set; }

        public int Length => Maximum - Minimum;

        public Range(int minimum, int maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public override string ToString()
        {
            return Minimum + "-" + Maximum;
        }
    }
}
