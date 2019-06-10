using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mcc.Command;
using mcc.Command.Tokens;
using mcc.Parser.ParseFilters;
using NUnit.Framework;

namespace McFuncCompiler.Test
{
    [TestFixture]
    public class ConstantParseFilterTest
    {
        private readonly ConstantParseFilter _filter;

        public ConstantParseFilterTest()
        {
            _filter = new ConstantParseFilter();
        }

        [Test]
        public void TestSingleConstantInCommand()
        {
            var arg = _filter.FilterArgument(new Argument("say #test"));

            Assert.That((arg.Tokens[0] as TextToken).Text, Is.EqualTo("say "));
            Assert.That((arg.Tokens[1] as ConstantToken).Constant, Is.EqualTo("test"));
        }

        [Test]
        public void TestConstantOnOwn()
        {
            var arg = _filter.FilterArgument(new Argument("#test"));
            
            Assert.That((arg.Tokens[0] as ConstantToken).Constant, Is.EqualTo("test"));
        }

        [Test]
        public void TestMultipleConstants()
        {
            var arg = _filter.FilterArgument(new Argument("#cmd #args1 #nbt"));

            Assert.That((arg.Tokens[0] as ConstantToken).Constant, Is.EqualTo("cmd"));
            Assert.That((arg.Tokens[1] as TextToken).Text, Is.EqualTo(" "));
            Assert.That((arg.Tokens[2] as ConstantToken).Constant, Is.EqualTo("args1"));
            Assert.That((arg.Tokens[3] as TextToken).Text, Is.EqualTo(" "));
            Assert.That((arg.Tokens[4] as ConstantToken).Constant, Is.EqualTo("nbt"));
        }
    }
}
