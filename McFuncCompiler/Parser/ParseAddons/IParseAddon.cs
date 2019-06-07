using System.Text;
using McFuncCompiler.Command;

namespace McFuncCompiler.Parser.ParseAddons
{
    public interface IParseAddon
    {
        ParseAddonResponse OnCharacter(Command.Command command, Argument argument, StringBuilder buffer, char c);
    }
}
