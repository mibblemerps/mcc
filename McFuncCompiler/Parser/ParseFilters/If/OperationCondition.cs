using System.Collections.Generic;
using System.Linq;
using McFuncCompiler.Command;

namespace McFuncCompiler.Parser.ParseFilters.If
{
    /// <summary>
    /// <p>Operation condition</p>
    /// <h3>Example</h3>
    /// <code>
    /// if $var1 &gt; $var2
    /// if $var1 == $var2
    /// if $var2 &lt; $var1
    /// </code>
    /// </summary>
    public class OperationCondition : Condition
    {
        public static readonly IReadOnlyList<string> Operators = new List<string>
        {
            "<", "<=", "=", ">", ">="
        };

        public Variable LeftVariable;
        public Variable RightVariable;
        public string Operation;

        public static OperationCondition Parse(List<Argument> arguments)
        {
            if (arguments.Count < 2)
                return null; // Not enough arguments

            var condition = new OperationCondition();

            int offset = condition.ParseNot(arguments);

            condition.LeftVariable = Variable.Parse(arguments[offset].GetAsText());
            if (condition.LeftVariable == null)
                return null;

            condition.Operation = arguments[offset + 1].GetAsText();
            if (!Operators.Contains(condition.Operation))
                return null; // Invalid operation

            condition.RightVariable = Variable.Parse(arguments[offset + 2].GetAsText());
            if (condition.RightVariable == null)
                return null;

            condition.LastIndex = offset + 2;

            return condition;
        }
    }
}
