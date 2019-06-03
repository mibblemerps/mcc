using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McFuncCompiler.Command;

namespace McFuncCompiler.ParseAddons
{
    public interface IParseAddon
    {
        ParseAddonResponse OnCharacter(Command.Command command, Argument argument, StringBuilder buffer, char c);
    }
}
