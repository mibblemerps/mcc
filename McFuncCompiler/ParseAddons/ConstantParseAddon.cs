using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using McFuncCompiler.Command;
using McFuncCompiler.Command.Tokens;

namespace McFuncCompiler.ParseAddons
{
    public class ConstantParseAddon : IParseAddon
    {
        private bool _inConstant;

        private bool _isAssigning;
        private string _assignmentConstantName;

        private StringBuilder _buffer = new StringBuilder();

        public ParseAddonResponse OnCharacter(Command.Command command, Argument argument, StringBuilder buffer, char c)
        {
            if (_isAssigning)
            {
                if (c == 0)
                {
                    // Finished defining constant
                    argument.Tokens.Add(new DefineConstantToken(_assignmentConstantName, _buffer.ToString()));
                    _buffer.Clear();
                    _isAssigning = false;
                    _inConstant = false;
                }
                else
                {
                    _buffer.Append(c);
                }
            }
            else if (_inConstant)
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
                        _assignmentConstantName = _buffer.ToString();
                        _buffer.Clear();
                        _isAssigning = true;
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
