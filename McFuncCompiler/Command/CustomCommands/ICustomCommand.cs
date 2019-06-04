namespace McFuncCompiler.Command.CustomCommands
{
    /// <summary>
    /// A command that should be run by the compiler, then stripped from the output.
    /// </summary>
    public interface ICustomCommand
    {
        /// <summary>
        /// Is this a compiler command? If so, the command will be stripped from the final output.
        /// </summary>
        bool CompilerOnly { get; }

        /// <summary>
        /// Check if the provided command matches this compiler command.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="command">Command to check</param>
        /// <returns></returns>
        bool DoesApply(BuildEnvironment env, Command command);

        /// <summary>
        /// Apply compiler command.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="command"></param>
        /// <returns>Apply result</returns>
        ApplyResult Apply(BuildEnvironment env, Command command);
    }
}
