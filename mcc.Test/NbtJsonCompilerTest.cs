using System.Collections.Generic;
using mcc.Parser.NbtJson;
using NUnit.Framework;

namespace McFuncCompiler.Test
{
    [TestFixture]
    public class NbtJsonCompilerTest
    {
        [Test]
        public void TestStringTag()
        {
            var withSpace = new NbtString("hello world");
            var withoutSpace = new NbtString("helloworld");
            var withQuotes = new NbtString("hello \"world\"!");
            var withLooseEscape = new NbtString("hello \\ world");

            Assert.That(withSpace.ToString(), Is.EqualTo("\"hello world\""));
            Assert.That(withoutSpace.ToString(), Is.EqualTo("helloworld"));
            Assert.That(withQuotes.ToString(), Is.EqualTo("\"hello \\\"world\\\"!\""));
            Assert.That(withLooseEscape.ToString(), Is.EqualTo("\"hello \\\\ world\""));
        }

        [Test]
        public void TestIntTag()
        {
            var intTag = new NbtNumber<int>(100);
            Assert.That(intTag.ToString(), Is.EqualTo("100"));
        }

        [Test]
        public void TestByteTag()
        {
            var byteTag = new NbtNumber<byte>(4);
            Assert.That(byteTag.ToString(), Is.EqualTo("4b"));
        }

        [Test]
        public void TestListTag()
        {
            var tag = new NbtList(new List<object>
            {
                new NbtString("hello world"),
                new NbtString("helloworld"),
                new NbtNumber<long>(621)
            });

            Assert.That(tag.ToString(), Is.EqualTo("[\"hello world\",helloworld,621l]"));
        }

        [Test]
        public void TestArrayTag()
        {
            var tag = new NbtList(new List<object>
            {
                new NbtNumber<int>(1),
                new NbtNumber<int>(2),
                new NbtNumber<int>(3)
            }, typeof(int));

            Assert.That(tag.ToString(), Is.EqualTo("[I;1,2,3]"));
        }

        [Test]
        public void TestCompoundTag()
        {
            var tag = new NbtCompoundTag
            {
                {"hi world", new NbtString("hello world")},
                {"int", new NbtNumber<int>(10)},
                {"byte", new NbtNumber<byte>(1)}
            };

            string compiled = tag.ToString();

            Assert.That(compiled, Is.EqualTo("{\"hi world\":\"hello world\",int:10,byte:1b}"));
        }
    }
}
