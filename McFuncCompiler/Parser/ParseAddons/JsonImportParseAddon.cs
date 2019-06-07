using System.Text;
using McFuncCompiler.Command;
using McFuncCompiler.Command.Tokens;

namespace McFuncCompiler.Parser.ParseAddons
{
    public class JsonImportParseAddon : IParseAddon
    {
        private bool _inJson;
        private StringBuilder _buffer = new StringBuilder();

        public ParseAddonResponse OnCharacter(Command.Command command, Argument argument, StringBuilder buffer, char c)
        {
            if (_inJson)
            {
                if (c == '`')
                {
                    // End of JSON import - flush it
                    argument.Tokens.Add(new JsonImportToken(_buffer.ToString()));

                    _inJson = false;
                    _buffer.Clear();
                }
                else
                {
                    _buffer.Append(c);
                }
                
                return ParseAddonResponse.Handled;
            }
            else
            {
                if (c == '`')
                {
                    // Start JSON import
                    _inJson = true;

                    return ParseAddonResponse.HandledClearBuffer;
                }
            }

            return ParseAddonResponse.NotHandled;
        }
    }
}
