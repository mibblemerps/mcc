using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFuncCompiler
{
    class Program
    {
        public static readonly Version Version = new Version(0, 1);

        static void Main(string[] args)
        {
            var parser = new Parser();
            McFunction mcFunction = parser.Parse("test:test", File.ReadAllText("../../TestFuncs/test.mcfunction"));
            mcFunction.Path = "test";

            var env = new BuildEnvironment("../../TestFuncs");
            env.Constants.Add("mcfunc_compiler_version", Version.ToString(2));

            mcFunction.Compile(env);

            Console.ReadKey(true);
        }
    }
}
