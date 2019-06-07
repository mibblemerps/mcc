using System.Text;
using System.Text.RegularExpressions;
using McFuncCompiler.Command;
using McFuncCompiler.Command.Tokens;

namespace McFuncCompiler.Parser.ParseAddons
{
    public class ConstantParseAddon : IParseAddon
    {
        private bool _inConstant;

        private StringBuilder _buffer = new StringBuilder();

        public ParseAddonResponse OnCharacter(Command.Command command, Argument argument, StringBuilder buffer, char c)
        {
            if (_inConstant)
            {
                // Check if character is suitable for a constant name
                if (Regex.IsMatch(c.ToString(), @"^[\w\d-_]+$"))
                {
                    _buffer.Append(c);
                }
                else
                {
                    if (command.Arguments.Count == 0)
                    {
                        // No existing tokens (constant being used on it's own so far). Do an assignment.
                        _buffer.Clear();
                    }
                    else
                    {
                        // Finish constant
                        argument.Tokens.Add(new ConstantToken(_buffer.ToString()));
                        _buffer.Clear();
                        _inConstant = false;
                    }
                }

                return ParseAddonResponse.Handled;
            }
            else if (c == '#')
            {
                // Start of constant
                _inConstant = true;
                return ParseAddonResponse.HandledClearBuffer;
            }

            return ParseAddonResponse.NotHandled;
        }
    }
}
