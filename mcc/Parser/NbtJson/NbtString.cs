using System.Text;

namespace mcc.Parser.NbtJson
{
    public class NbtString
    {
        public string Value { get; }

        public NbtString(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            if (NbtJsonParser.IsValidUnquotedString.IsMatch(Value))
            {
                // All characters are valid and don't need to be quoted
                return Value;
            }

            var builder = new StringBuilder("\"");

            int cursor = 0;
            while (cursor < Value.Length)
            {
                char c = Value[cursor++];

                if (c == '\\' || c == '"')
                {
                    // Needs escaping
                    builder.Append("\\");
                }

                if (c == '\n')
                {
                    // New line character replaced with new line string representation
                    builder.Append("\\n");
                    continue;
                }

                builder.Append(c);
            }

            builder.Append("\"");

            return builder.ToString();
        }
    }
}
