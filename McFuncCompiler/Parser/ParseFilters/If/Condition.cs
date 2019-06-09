using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McFuncCompiler.Command;

namespace McFuncCompiler.Parser.ParseFilters.If
{
    /// <summary>
    /// If statement condition. If statements consist of one or more of these.
    /// </summary>
    public class Condition
    {
        public bool Not = false;

        /// <summary>
        /// For the parser. The last argument index that was part of this condition's syntax.
        /// </summary>
        public int LastIndex;

        public string CustomStatement { get; }

        public Condition(string customStatement)
        {
            CustomStatement = customStatement;
        }

        public Condition()
        {
        }

        public virtual string Compile()
        {
            return CustomStatement;
        }

        /// <summary>
        /// <p>Parse whether this condition is inverted.</p>
        /// <p>The result is stored in <see cref="Not"/>.</p>
        /// </summary>
        /// <param name="arguments">List of arguments</param>
        /// <returns>Offset where the next arguments should read from to avoid reading the not statement(s).</returns>
        public int ParseNot(List<Argument> arguments)
        {
            int i;
            for (i = 1; i < arguments.Count; i++)
            {
                var argument = arguments[i];

                if (argument.GetAsText().ToLower() == "not")
                {
                    Not = !Not;
                }
                else
                {
                    break;
                }
            }

            return i;
        }
    }
}
