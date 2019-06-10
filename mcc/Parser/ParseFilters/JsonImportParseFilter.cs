using System.Text.RegularExpressions;
using mcc.Command;
using mcc.Command.Tokens;

namespace mcc.Parser.ParseFilters
{
    public class JsonImportParseFilter : IParseFilter
    {
        private static readonly Regex JsonImportRegex = new Regex(@"(?:^|[^\\])(`([\w-:.]+)`)", RegexOptions.Compiled);

        public Command.Command Filter(Command.Command command)
        {
            return command;
        }

        public Argument FilterArgument(Argument argument)
        {
            foreach (IToken token in argument.Tokens)
            {
                if (!(token is TextToken textToken)) continue;

                Match match = JsonImportRegex.Match(textToken.Text);
                if (!match.Success) continue;

                // Remove constant from text token, create a new constant token, and create appropriate surrounding tokens
                string after = textToken.Text.Substring(match.Groups[1].Index + match.Groups[1].Length);
                textToken.Text = textToken.Text.Remove(match.Groups[1].Index);
                if (textToken.Text.Length == 0)
                    argument.Tokens.Remove(textToken); // If text token is empty, just remove it

                argument.Tokens.Add(new JsonImportToken(match.Groups[2].Value));

                if (after.Length > 0)
                    argument.Tokens.Add(new TextToken(after));


                // Restart this whole process from scratch incase of more constants
                return FilterArgument(argument);
            }

            return argument;
        }
    }
}
