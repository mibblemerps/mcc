using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using McFuncCompiler.Command;
using McFuncCompiler.Command.Tokens;

namespace McFuncCompiler.Parser.ParseFilters
{
    public class ConstantParseFilter : IParseFilter
    {
        private static readonly Regex ConstantRegex = new Regex(@"(?:^|[^\\])#([\w-]+)", RegexOptions.Compiled);

        public Command.Command Filter(Command.Command command)
        {
            return command;
        }

        public Argument FilterArgument(Argument argument)
        {
            foreach (IToken token in argument.Tokens)
            {
                if (!(token is TextToken textToken)) continue;

                Match match = ConstantRegex.Match(textToken.Text);
                if (!match.Success) continue;
                
                // Remove constant from text token, create a new constant token, and create appropriate surrounding tokens
                string after = textToken.Text.Substring(match.Groups[1].Index + match.Groups[1].Length);
                textToken.Text = textToken.Text.Remove(match.Groups[1].Index - 1);
                if (textToken.Text.Length == 0)
                    argument.Tokens.Remove(textToken); // No text left in token, remove entirely
                argument.Tokens.Add(new ConstantToken(match.Groups[1].Value));
                if (after.Length > 0)
                    argument.Tokens.Add(new TextToken(after));

                // Restart this whole process from scratch incase of more constants
                return FilterArgument(argument);
            }

            return argument;
        }
    }
}
