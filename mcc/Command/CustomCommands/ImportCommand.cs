namespace mcc.Command.CustomCommands
{
    public class ImportCommand : ICustomCommand
    {
        public bool CompilerOnly => true;

        public bool DoesApply(BuildEnvironment env, Command command)
        {
            return command.GetCommandName() == "import";
        }

        public ApplyResult Apply(BuildEnvironment env, Command command)
        {
            string id = command.Arguments[1].GetAsText();
            string path = env.GetPath(id, "mcfunction");

            Logger.Debug($"Importing {id}...");

            var parser = new Parser.Parser(env);
            McFunction mcFunction = parser.Parse(id);
            mcFunction.Compile(env, false);

            return new ApplyResult(true, mcFunction.Commands);
        }
    }
}
