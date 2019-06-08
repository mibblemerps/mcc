using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McFuncCompiler.Parser.NbtJson;
using NUnit.Framework;

namespace McFuncCompiler.Test
{
    [TestFixture]
    public class NbtJsonParserTest
    {
        [Test]
        public void TestUnquotedStringTag()
        {
            NbtString str = NbtJsonParser.Parse("hello_world") as NbtString;
            Assert.That(str, Is.Not.Null);
            Assert.That(str.Value, Is.EqualTo("hello_world"));
        }

        [Test]
        public void TestQuotedStringTag()
        {
            NbtString str = NbtJsonParser.Parse("\"hello world\"") as NbtString;
            Assert.That(str, Is.Not.Null);
            Assert.That(str.Value, Is.EqualTo("hello world"));
        }

        [Test]
        public void TestQuotedStringDanglingEscape()
        {
            Assert.That(() =>
            {
                NbtJsonParser.Parse("\"hello \\ world\"");
            }, Throws.Exception);
        }

        [Test]
        public void TestIntTag()
        {
            NbtNumber<int> number = (NbtNumber<int>) NbtJsonParser.Parse("10");
            Assert.That(number.Value, Is.EqualTo(10));
        }

        [Test]
        public void TestByteTag()
        {
            NbtNumber<byte> number = (NbtNumber<byte>)NbtJsonParser.Parse("1b");
            Assert.That(number.Value, Is.EqualTo((byte) 1));
        }

        [Test]
        public void TestNbtCompoundTag()
        {
            NbtCompoundTag nbt = (NbtCompoundTag) NbtJsonParser.Parse("{string1:test,string2:\"test\",byte:1b,int:2}");

            Assert.That(nbt.GetString("string1").Value, Is.EqualTo("test"));
            Assert.That(nbt.GetString("string2").Value, Is.EqualTo("test"));
            Assert.That(nbt.GetByte("byte").Value, Is.EqualTo((byte) 1));
            Assert.That(nbt.GetInt("int").Value, Is.EqualTo(2));
        }

        [Test]
        public void TestNbtListTag()
        {
            NbtList list = (NbtList) NbtJsonParser.Parse("[110f,40f]");
            
            Assert.That(list.Type, Is.Null, "Regular lists don't have types, only typed arrays");
            Assert.That(((NbtNumber<float>) list[0]).Value, Is.EqualTo(110f));
            Assert.That(((NbtNumber<float>)list[1]).Value, Is.EqualTo(40f));
        }

        [Test]
        public void TestNbtArrayTag()
        {
            NbtList array = (NbtList) NbtJsonParser.Parse("[L;32l,64l,128l]");

            Assert.That(array.Type, Is.EqualTo(typeof(long)));
            Assert.That(((NbtNumber<long>) array[0]).Value, Is.EqualTo((long) 32));
            Assert.That(((NbtNumber<long>) array[1]).Value, Is.EqualTo((long) 64));
            Assert.That(((NbtNumber<long>) array[2]).Value, Is.EqualTo((long) 128));
        }
    }
}
