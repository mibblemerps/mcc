using System;

namespace McFuncCompiler
{
    class Program
    {
        public static readonly Version Version = new Version(0, 1);

        static void Main(string[] args)
        {
            var env = new BuildEnvironment("../../TestData/datapack");
            env.OutputPath = "../../TestData/datapack_compiled";
            env.Constants.Add("mcfunc_compiler_version", Version.ToString(2));

            DateTime startParse = DateTime.UtcNow;

            var parser = new Parser.Parser(env);
            McFunction mcFunction = parser.Parse("test_data:test");

            TimeSpan parseTime = DateTime.UtcNow - startParse;
            Logger.Info($"Parsed in {parseTime.Milliseconds}ms.");

            DateTime startCompile = DateTime.UtcNow;

            mcFunction.Compile(env);

            TimeSpan compileTime = DateTime.UtcNow - startCompile;
            Logger.Info($"Compiled in {compileTime.Milliseconds}ms.");

            mcFunction.Save(env);

            Console.ReadKey(true);
        }
    }
}
