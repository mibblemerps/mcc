using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFuncCompiler.Command.CompilerCommands
{
    /// <summary>
    /// A command that should be run by the compiler, then stripped from the output.
    /// </summary>
    public interface ICompilerCommand
    {
        /// <summary>
        /// Check if the provided command matches this compiler command.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="command">Command to check</param>
        /// <returns></returns>
        bool DoesApply(BuildEnvironment env, Command command);

        /// <summary>
        /// Apply compiler command.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="command"></param>
        void Apply(BuildEnvironment env, Command command);
    }
}
