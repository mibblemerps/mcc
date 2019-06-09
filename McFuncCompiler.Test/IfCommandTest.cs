using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McFuncCompiler.Command;
using McFuncCompiler.Command.CustomCommands;
using McFuncCompiler.Command.Tokens;
using McFuncCompiler.Parser.ParseFilters.If;
using NUnit.Framework;

namespace McFuncCompiler.Test
{
    [TestFixture]
    public class IfCommandTest
    {
        private IfCommand _ifCommand;
        private BuildEnvironment _env;
        private Command.Command _successCommand;

        public IfCommandTest()
        {
            _ifCommand = new IfCommand();
            _env = new BuildEnvironment(null);

            _successCommand = new Command.Command("test");
        }

        [Test]
        public void TestMatchesIf()
        {
            string result = CreateIfCommand(new MatchesCondition {LeftVariable = new Variable("var", null, null), Matches = "5..10"});
            Assert.That(result, Is.EqualTo("execute if score var globals matches 5..10 run test"));
        }

        [Test]
        public void TestOperationIf()
        {
            string result = CreateIfCommand(new OperationCondition {LeftVariable = new Variable("left", null, null), RightVariable = new Variable("right", null, null), Operation = "="});
            Assert.That(result, Is.EqualTo("execute if score left globals = right globals run test"));
        }

        [Test]
        public void TestMultipleConditions()
        {
            string result = CreateIfCommand(
                new MatchesCondition { LeftVariable = new Variable("var", null, null), Matches = "5..10" },
                new OperationCondition { LeftVariable = new Variable("left", null, null), RightVariable = new Variable("right", null, null), Operation = "=" }
                );

            Assert.That(result, Is.EqualTo("execute if score var globals matches 5..10 if score left globals = right globals run test"));
        }

        [Test]
        public void TestNots()
        {
            string result = CreateIfCommand(new MatchesCondition { LeftVariable = new Variable("var", null, null), Matches = "5..10", Not = true });
            Assert.That(result.StartsWith("execute unless "), Is.True);
        }

        protected string CreateIfCommand(params Condition[] conditions)
        {
            var command = new Command.Command();
            command.Arguments.Add(new Argument(new IfToken(conditions.ToList(), _successCommand)));

            command = _ifCommand.Apply(_env, command).AddCommands.FirstOrDefault();
            Assert.That(command, Is.Not.Null, "If statement didn't parse");

            StringBuilder builder = new StringBuilder();
            foreach (Argument argument in command.Arguments)
                builder.Append(argument.Compile(_env) + " ");

            return builder.ToString().Substring(0, builder.Length - 1);
        }
    }
}
