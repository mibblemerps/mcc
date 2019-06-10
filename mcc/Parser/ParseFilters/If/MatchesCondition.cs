using System.Collections.Generic;
using System.Text.RegularExpressions;
using mcc.Command;

namespace mcc.Parser.ParseFilters.If
{
    /// <summary>
    /// <p>Matches condition</p>
    /// <h3>Example</h3>
    /// <code>
    /// if $var 5..10
    /// if $var ..32
    /// if $var 5..
    /// if $var 0
    /// </code>
    /// </summary>
    public class MatchesCondition : Condition
    {
        /// <summary>
        /// <p>Matches a match range (eg; 10.., ..5, 1..5, 4)</p>
        /// <p>Note this isn't a perfect Regex and will match some things that wouldn't be valid match syntaxes. It's mostly a "sanity check".</p>
        /// </summary>
        private static readonly Regex MatchRegex = new Regex(@"^(?:\.\.){0,2}[\d]+(?:\.\.){0,2}[\d]*$", RegexOptions.Compiled);

        public Variable LeftVariable;
        public string Matches;

        public static MatchesCondition Parse(List<Argument> arguments)
        {
            if (arguments.Count < 2)
                return null; // Not enough arguments

            var condition = new MatchesCondition();

            int offset = condition.ParseNot(arguments);

            condition.LeftVariable = Variable.Parse(arguments[offset].GetAsText());
            if (condition.LeftVariable == null)
                return null;

            condition.Matches = arguments[offset + 1].GetAsText();
            if (!MatchRegex.IsMatch(condition.Matches))
                return null; // Not a valid match syntax

            condition.LastIndex = offset + 1;

            return condition;
        }
    }
}
