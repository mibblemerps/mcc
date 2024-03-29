﻿using mcc.Command;
using mcc.Command.Tokens;
using mcc.Parser.ParseFilters;
using NUnit.Framework;

namespace McFuncCompiler.Test
{
    [TestFixture]
    public class FunctionBlockParseFilterTest
    {
        private readonly FunctionBlockParseFilter _filter;

        public FunctionBlockParseFilterTest()
        {
            _filter = new FunctionBlockParseFilter();
        }

        [Test]
        public void TestOpenFunctionBlockParse()
        {
            var cmd = new Command("execute", "run", "(");

            cmd = _filter.Filter(cmd);

            Assert.That(cmd.Arguments[2].Tokens[0], Is.InstanceOf(typeof(OpenFunctionBlockToken)));
        }

        [Test]
        public void TestCloseFunctionBlockParse()
        {
            var cmd = new Command(")");

            cmd = _filter.Filter(cmd);

            Assert.That(cmd.Arguments[0].Tokens[0], Is.InstanceOf(typeof(CloseFunctionBlockToken)));
        }

        [Test]
        public void TestCloseFunctionBlockMustBeOnlyArgument()
        {
            var cmd = new Command("say", ")");

            cmd = _filter.Filter(cmd);

            Assert.That(cmd.Arguments[1].Tokens[0], Is.InstanceOf(typeof(TextToken)));
        }
    }
}
