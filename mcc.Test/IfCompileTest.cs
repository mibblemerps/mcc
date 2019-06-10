using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mcc;
using mcc.Command;
using mcc.Command.Tokens;
using mcc.Parser.ParseFilters.If;
using NUnit.Framework;

namespace McFuncCompiler.Test
{
    [TestFixture]
    public class IfCompileTest
    {
        private BuildEnvironment _env;

        public IfCompileTest()
        {
            _env = new BuildEnvironment(null);
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
            IfToken token = new IfToken(new List<Condition>(conditions));
            return token.Compile(_env) + " test";
        }
    }
}
