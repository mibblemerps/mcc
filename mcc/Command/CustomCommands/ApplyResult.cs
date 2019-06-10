using System.Collections.Generic;

namespace mcc.Command.CustomCommands
{
    /// <summary>
    /// Result from applying a command, and a directive at what should be done to the command that was applied.
    /// </summary>
    public class ApplyResult
    {
        public bool StripFromOutput;

        public List<Command> AddCommands;
        
        /// <param name="stripFromOutput">Strip the command from output? Useful for compiler commands like <i>define</i>.</param>
        /// <param name="addCommands">List of commands to be inserted after this command (or in it's place if <see cref="StripFromOutput"/> is true.</param>
        public ApplyResult(bool stripFromOutput, List<Command> addCommands = null)
        {
            StripFromOutput = stripFromOutput;
            AddCommands = addCommands ?? new List<Command>();
        }
    }
}
