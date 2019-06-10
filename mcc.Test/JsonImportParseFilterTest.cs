using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mcc.Command;
using mcc.Command.Tokens;
using mcc.Parser.ParseFilters;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace McFuncCompiler.Test
{
    [TestFixture]
    public class JsonImportParseFilterTest
    {
        private JsonImportParseFilter _filter;

        public JsonImportParseFilterTest()
        {
            _filter = new JsonImportParseFilter();
        }

        [Test]
        public void TestJsonImport()
        {
            var argument = new Argument("`test:villager.nbt`");

            Assert.That((_filter.FilterArgument(argument).Tokens[0] as JsonImportToken).Id, Is.EqualTo("test:villager.nbt"));
        }
    }
}
