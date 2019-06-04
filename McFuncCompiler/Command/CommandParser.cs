using System.Collections.Generic;
using System.Text;
using McFuncCompiler.Parser;

namespace McFuncCompiler.Command
{
    public static class CommandParser
    {
        /// <summary>
        /// Splits a command into it's component arguments.
        /// </summary>
        /// <param name="line">Command string</param>
        /// <returns>Command parts, or null if no command exists in the provided string.</returns>
        public static List<string> Parse(string line)
        {
            line = line.Trim();

            // Ignore comments and empty lines
            if (line.StartsWith("#") || line.Length == 0)
                return null;

            var parts = new List<string>();

            // Parsing state
            ParseState state = ParseState.Default;
            bool escape = false;
            StringBuilder buffer = new StringBuilder();

            foreach (char c in line.Trim() + " ")
            {
                if (escape)
                {
                    // Character escaped, just output it and move on.
                    buffer.Append(c);
                    escape = false;
                    continue;
                }

                if (c == '\\')
                {
                    escape = true;
                }

                switch (state)
                {
                    case ParseState.Default:
                        // Space ( ) - denotes next argument
                        if (c == ' ')
                        {
                            parts.Add(buffer.ToString());
                            
                            buffer.Clear();
                            continue;
                        }

                        // Check if string block has been opened
                        if (c == '"')
                        {
                            buffer.Append(c);
                            escape = false;
                            state = ParseState.String;
                            continue;
                        }

                        // Not a special character, append to regular buffer
                        buffer.Append(c);

                        break;

                    case ParseState.String:
                        // Check for end of string block
                        if (c == '"')
                        {
                            buffer.Append(c);
                            state = ParseState.Default;
                            continue;
                        }

                        // Output to buffer
                        buffer.Append(c);

                        break;
                }
            }

            return parts;
        }
    }
}
