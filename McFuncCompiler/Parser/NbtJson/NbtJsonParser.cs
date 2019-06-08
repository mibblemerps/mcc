using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;

namespace McFuncCompiler.Parser.NbtJson
{
    /// <summary>
    /// A reimplementation of Minecraft's weird non-standard JSON NBT parser.
    /// </summary>
    public class NbtJsonParser
    {
        private const RegexOptions RegexOptions =
            System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.Multiline;

        // Value regex patterns
        public static Regex LongRegex = new Regex("^[-+]?(?:0|[1-9][0-9]*)l$", RegexOptions);
        public static Regex IntRegex = new Regex("^[-+]?(?:0|[1-9][0-9]*)$", RegexOptions);
        public static Regex ShortRegex = new Regex("^[-+]?(?:0|[1-9][0-9]*)s$", RegexOptions);
        public static Regex ByteRegex = new Regex("^[-+]?(?:0|[1-9][0-9]*)b$", RegexOptions);
        public static Regex FloatRegex = new Regex("^[-+]?(?:[0-9]+[.]?|[0-9]*[.][0-9]+)(?:e[-+]?[0-9]+)?f$", RegexOptions);
        public static Regex DoubleNoSuffixRegex = new Regex(@"^[-+]?(?:[0-9]+[.]|[0-9]*[.][0-9]+)(?:e[-+]?[0-9]+)?$", RegexOptions);
        public static Regex DoubleRegex = new Regex("^[-+]?(?:[0-9]+[.]?|[0-9]*[.][0-9]+)(?:e[-+]?[0-9]+)?d$", RegexOptions);

        public static Regex AllowedInKey = new Regex(@"[\d\w_\-\.\+]", RegexOptions.Compiled);
        public static Regex IsValidUnquotedString = new Regex(@"^[\d\w_\-\.\+]+$", RegexOptions.Compiled);

        private string _json;
        private int _cursor;

        private NbtJsonParser(string json)
        {
            _json = json;
        }

        public static object Parse(string json)
        {
            return new NbtJsonParser(json).ReadSingleValue();
        }

        /// <summary>
        /// Read a single value (any type; compound, list, literal, etc..) and throw an exception if there is any trailing data.
        /// </summary>
        private object ReadSingleValue()
        {
            object value = ReadValue();

            SkipWhitespace();

            if (CanRead())
                throw new NbtJsonException("Trailing data found");

            return value;
        }

        /// <summary>
        /// Read a NBT compound tag (like a JSON object).
        /// </summary>
        private NbtCompoundTag ReadTag()
        {
            Expect('{');
            SkipWhitespace();

            var tag = new NbtCompoundTag();

            while (CanRead() && Peek() != '}')
            {
                string key = ReadKey();

                if (key.Length == 0)
                    throw new NbtJsonException($"Expected non-empty key");

                Expect(':');

                tag[key] = ReadValue();

                if (!ReadElementSeperator())
                    break; // End of tag contents

                if (!CanRead())
                    throw new NbtJsonException("Expected key");
            }

            Expect('}');

            return tag;
        }

        /// <summary>
        /// Read NBT compound tag key.
        /// </summary>
        private string ReadKey()
        {
            SkipWhitespace();

            if (!CanRead())
                throw new NbtJsonException("Expected key but reached end of string");

            return Peek() == '"' ? ReadQuotedString() : ReadUnquotedString();
        }

        private string ReadUnquotedString()
        {
            int start;

            for (start = _cursor; CanRead() && AllowedInKey.IsMatch(Peek().ToString()); ++_cursor)
            {
            }

            return _json.Substring(start, _cursor - start);
        }

        private string ReadQuotedString()
        {
            int start = _cursor++;

            bool escape = false;

            var builder = new StringBuilder();

            while (CanRead())
            {
                char c = Pop();

                if (escape)
                {
                    if (c != '\\' && c != '"')
                        throw new NbtJsonException($"Invalid escape of \"{c}\"");

                    builder.Append(c);

                    escape = false;
                }
                else
                {
                    if (c == '\\') {
                        escape = true;
                    }
                    else if (c == '"')
                    {
                        return builder.ToString();
                    }
                    else
                    {
                        builder.Append(c);
                    }
                }
            }

            throw new NbtJsonException($"Unterminated string starting at {start}");
        }

        /// <summary>
        /// Read any value; compound tags, lists or any typed value.
        /// </summary>
        private object ReadValue()
        {
            SkipWhitespace();
            
            if (!CanRead())
                throw new NbtJsonException("Expected value");

            char c = Peek();

            switch (c)
            {
                case '{':
                    return ReadTag();
                case '[':
                    return ReadList();
                default:
                    return ReadTypedValue();
            }
        }

        /// <summary>
        /// Read a typed value. Typed values include: <i>string, long, int, short, byte, float, and double</i>.
        /// </summary>
        private object ReadTypedValue()
        {
            SkipWhitespace();

            if (Peek() == '"')
            {
                return new NbtString(ReadQuotedString());
            }
            else
            {
                string value = ReadUnquotedString();

                if (value.Length == 0)
                    throw new NbtJsonException("Expected value");

                return ToType(value);
            }
        }

        /// <summary>
        /// Convert string to the correct typed NBT datatype.
        /// </summary>
        /// <param name="value">String value</param>
        private object ToType(string value)
        {
            // Attempt to match and parse to a data type
            try
            {
                if (LongRegex.IsMatch(value))
                    return new NbtNumber<long>(long.Parse(value.Substring(0, value.Length - 1)));

                if (IntRegex.IsMatch(value))
                    return new NbtNumber<int>(int.Parse(value));

                if (ShortRegex.IsMatch(value))
                    return new NbtNumber<short>(short.Parse(value.Substring(0, value.Length - 1)));

                if (ByteRegex.IsMatch(value))
                    return new NbtNumber<byte>(byte.Parse(value.Substring(0, value.Length - 1)));

                if (FloatRegex.IsMatch(value))
                    return new NbtNumber<float>(float.Parse(value.Substring(0, value.Length - 1)));

                if (DoubleRegex.IsMatch(value))
                    return new NbtNumber<short>(short.Parse(value.Substring(0, value.Length - 1)));

                if (DoubleNoSuffixRegex.IsMatch(value))
                    return new NbtNumber<short>(short.Parse(value));
            }
            catch (FormatException) { }

            // If didn't match any specific type, return as string.
            return new NbtString(value);
        }

        /// <summary>
        /// <p>Read a list tag. Similar to array tag except not initially locked to one type (but still only can contain one type).</p>
        /// <h3>Example</h3>
        /// <code>[110f,0f]</code>
        /// </summary>
        private NbtList ReadList()
        {
            if (CanRead(2) && Peek(1) != '"' && Peek(2) == ';')
            {
                return ReadArrayTag();
            }
            else
            {
                return ReadListTag();
            }
        }

        /// <summary>
        /// <p>Read an array tag</p>
        /// <h3>Example</h3>
        /// <code>[B;1b,2b,3b]</code>
        /// </summary>
        private NbtList ReadArrayTag()
        {
            Expect('[');

            char c = Pop();
            Pop();
            SkipWhitespace();

            if (!CanRead())
                throw new NbtJsonException("Expected value");

            if (c == 'L')
                return new NbtList(ReadListTag(true, typeof(long), true), typeof(long));

            if (c == 'I')
                return new NbtList(ReadListTag(true, typeof(int), true), typeof(int));

            if (c == 'B')
                return new NbtList(ReadListTag(true, typeof(byte), true), typeof(byte));

            throw new NbtJsonException($"Invalid array type \"{c}\"");
        }

        private NbtList ReadListTag(bool skipStart = false, Type listType = null, bool enforceListType = false)
        {
            if (!skipStart)
            {
                Expect('[');
                SkipWhitespace();
            }

            if (!CanRead())
                throw new NbtJsonException("Expected value");

            List<object> list = new List<object>();

            while (Peek() != ']')
            {
                dynamic value = ReadValue();

                Type type = value.GetType();
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(NbtNumber<>))
                    type = value.Type;

                if (listType == null)
                {
                    // Set this list's type to the type of the first value
                    listType = value.GetType();
                }
                else if (enforceListType && type != listType)
                {
                    throw new NbtJsonException($"Attempt to insert {value.GetType().Name} into a {listType.Name} list");
                }

                list.Add(value);

                if (!ReadElementSeperator())
                {
                    // No element seperator - end of list
                    break;
                }

                if (!CanRead())
                    throw new NbtJsonException("Expected value");
            }

            Expect(']');

            return new NbtList(list);
        }

        private bool ReadElementSeperator()
        {
            SkipWhitespace();

            if (CanRead() && Peek() == ',')
            {
                _cursor++;
                SkipWhitespace();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Throw an exception if the next character isn't the expected character.
        /// </summary>
        /// <param name="expect">Character to expect</param>
        private void Expect(char expect)
        {
            SkipWhitespace();

            if (!CanRead())
                throw new NbtJsonException($"Expected {expect} but reached end of string");

            char c = Peek();
            if (c != expect)
                throw new NbtJsonException($"Expected {expect} but got \"{c}\"");

            _cursor++;
        }

        /// <summary>
        /// Increment cursor until next character isn't whitespace
        /// </summary>
        private void SkipWhitespace()
        {
            while (CanRead() && char.IsWhiteSpace(Peek()))
                _cursor++;
        }

        private char Pop()
        {
            return _json[_cursor++];
        }

        private char Peek(int offset = 0)
        {
            return _json[_cursor + offset];
        }

        private bool CanRead(int length = 0)
        {
            return _cursor + length < _json.Length;
        }
    }
}
