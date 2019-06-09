using System.Text.RegularExpressions;

namespace McFuncCompiler.Command
{
    public class Variable
    {
        public const string VarRegex = @"^([\w\d-_]*)\$([^\s=]+)$";

        public string Name;
        public string Scoreboard;
        public int? Value;

        public Variable(string name, string scoreboard, int? value)
        {
            Name = name;
            Scoreboard = scoreboard;
            Value = value;
        }

        public static Variable Parse(string str)
        {
            Match match = Regex.Match(str, VarRegex);
            if (!match.Success)
                return null;

            string scoreboard = match.Groups[1].Value;
            string name = match.Groups[2].Value;

            if (scoreboard == "") scoreboard = null;

            return new Variable(name, scoreboard, null);
        }
    }
}
