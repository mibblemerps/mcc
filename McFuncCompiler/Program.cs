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
        static void Main(string[] args)
        {
            var parser = new Parser();
            McFunction mcFunction = parser.Parse(File.ReadAllText("../../TestFuncs/test.mcfunction"));

            var env = new BuildEnvironment("../../TestFuncs");
            env.Constants.Add("ping", "pong");

            Console.WriteLine(mcFunction.Compile(env));

            Console.ReadKey(true);
        }
    }
}
