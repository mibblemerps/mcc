using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McFuncCompiler.Command;

namespace McFuncCompiler.Parser.ParseFilters
{
    /// <summary>
    /// Provides an interface for filters to operate on the source code as it's being parsed.
    /// </summary>
    public interface IParseFilter
    {
        /// <summary>
        /// Perform filter on command
        /// </summary>
        /// <param name="command">Command being filtered</param>
        /// <returns>Modified, new or unchanged command</returns>
        Command.Command Filter(Command.Command command);

        /// <summary>
        /// Filter the argument
        /// </summary>
        /// <param name="argument"></param>
        /// <returns>Modified, new or unchanged argument</returns>
        Argument FilterArgument(Argument argument);
    }
}
