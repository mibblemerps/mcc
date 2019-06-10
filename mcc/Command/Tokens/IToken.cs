namespace mcc.Command.Tokens
{
    public interface IToken
    {
        /// <summary>
        /// Evaluate the token into a string to can be used to formulate a valid Minecraft command.
        /// </summary>
        string Compile(BuildEnvironment env);
    }
}
