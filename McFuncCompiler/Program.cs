using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using CommandLine;

namespace McFuncCompiler
{
    class Program
    {
        public static readonly Version Version = new Version(0, 1);

        static void Main(string[] args)
        {
#if DEBUG
            if (args.Length == 0)
            {
                Console.WriteLine("[DEBUG] Pleae enter arguments...");
                Console.Write("> ");
                args = Console.ReadLine().Split(' ');
            }
#endif

            CommandLine.Parser.Default.ParseArguments<Options.CompileDatapack, Options.CompileFile>(args)
                .WithParsed<Options.CompileDatapack>(CompileDatapack.Compile)
                .WithParsed<Options.CompileFile>(CompileFile);

#if DEBUG
            Console.ReadKey(true);
#endif
        }

        static void CompileFile(Options.CompileFile options)
        {
            Console.WriteLine("Feature not yet implemented.");
        }
    }
}
