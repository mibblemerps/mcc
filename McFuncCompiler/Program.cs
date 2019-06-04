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

            var parser = new Parser.Parser(env);
            McFunction mcFunction = parser.Parse("test_data:test");

            mcFunction.Compile(env);

            mcFunction.Save(env);

            Console.ReadKey(true);
        }
    }
}
