using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McFuncCompiler.Command.Tokens;

namespace McFuncCompiler.Command
{
    /// <summary>
    /// Represents a singular command.
    /// </summary>
    public class Command
    {
        public List<Argument> Arguments = new List<Argument>();

        public Command()
        {
        }

        public Command(params string[] args)
        {
            foreach (string arg in args)
            {
                Arguments.Add(new Argument(arg));
            }
        }

        public Command(List<Argument> args)
        {
            Arguments = args;
        }

        /// <summary>
        /// Get the name of the command.<br />
        /// This requires the first token to be a <see cref="TextToken"/>.
        /// </summary>
        /// <returns></returns>
        public string GetCommandName()
        {
            if (Arguments.Count == 0) return null;
            if (Arguments[0].Tokens.Count == 0) return null;

            return Arguments[0].GetAsText();
        }
    }
}
