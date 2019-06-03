using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McFuncCompiler.Command;

namespace McFuncCompiler
{
    public class McFunction
    {
        public List<Command.Command> Commands = new List<Command.Command>();

        public string Compile(BuildEnvironment env)
        {
            var builder = new StringBuilder();

            foreach (Command.Command command in Commands)
            {
                var argBuilder = new StringBuilder(); 
                foreach (Argument arg in command.Arguments)
                {
                    argBuilder.Append(arg.Compile(env) + " ");
                }

                builder.Append(argBuilder.ToString().Trim() + "\n");
            }

            return builder.ToString();
        }
    }
}
