using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McFuncCompiler.Command.Tokens;
using McFuncCompiler.Parser.ParseFilters.If;
using NUnit.Framework;

namespace McFuncCompiler.Test
{
    [TestFixture]
    public class IfParseFilterTest
    {
        private IfParseFilter _filter;

        public IfParseFilterTest()
        {
            _filter = new IfParseFilter();
        }

        [Test]
        public void TestSimpleMatchesIf()
        {
            var command = new Command.Command("if", "$var", "1..2", "say", "test");
            var output = _filter.Filter(command);

            IfToken token = output.Arguments[0].Tokens[0] as IfToken;
            Assert.That(token, Is.Not.Null);

            Assert.That(token.Conditions[0].Not, Is.False);
            Assert.That(token.Conditions[0], Is.InstanceOf(typeof(MatchesCondition)));

            MatchesCondition matches = token.Conditions[0] as MatchesCondition;

            Assert.That(matches.LeftVariable.Name, Is.EqualTo("var"));
            Assert.That(matches.Matches, Is.EqualTo("1..2"));
        }

        [Test]
        public void TestSimpleOperationIf()
        {
            var command = new Command.Command("if", "$left", ">", "$right", "say", "test");
            var output = _filter.Filter(command);

            IfToken token = output.Arguments[0].Tokens[0] as IfToken;
            Assert.That(token, Is.Not.Null);

            Assert.That(token.Conditions[0].Not, Is.False);
            Assert.That(token.Conditions[0], Is.InstanceOf(typeof(OperationCondition)));

            OperationCondition operation = token.Conditions[0] as OperationCondition;

            Assert.That(operation.LeftVariable.Name, Is.EqualTo("left"));
            Assert.That(operation.Operation, Is.EqualTo(">"));
            Assert.That(operation.RightVariable.Name, Is.EqualTo("right"));
        }

        [Test]
        public void TestNots()
        {
            var command = new Command.Command("if", "not", "not", "not", "$var", "1", "say", "test");
            var output = _filter.Filter(command);

            IfToken token = output.Arguments[0].Tokens[0] as IfToken;
            Assert.That(token, Is.Not.Null);

            Assert.That(token.Conditions[0].Not, Is.True);
        }

        [Test]
        public void TestMultipleConditions()
        {
            var command = new Command.Command("if", "$var", "1", "&&", "$left", "=", "$right", "say", "test");
            var output = _filter.Filter(command);

            IfToken token = output.Arguments[0].Tokens[0] as IfToken;
            Assert.That(token, Is.Not.Null);

            Assert.That(token.Conditions.Count, Is.EqualTo(2));

            Assert.That(((MatchesCondition) token.Conditions[0]).LeftVariable.Name, Is.EqualTo("var"));
            Assert.That(((MatchesCondition)token.Conditions[0]).Matches, Is.EqualTo("1"));

            Assert.That(((OperationCondition) token.Conditions[1]).LeftVariable.Name, Is.EqualTo("left"));
            Assert.That(((OperationCondition)token.Conditions[1]).Operation, Is.EqualTo("="));
            Assert.That(((OperationCondition)token.Conditions[1]).RightVariable.Name, Is.EqualTo("right"));
        }
    }
}
