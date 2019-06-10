using System;
using System.Diagnostics;
using System.Reflection;
using CommandLine;

namespace mcc
{
    class Program
    {
        /// <summary>
        /// Informational version from the assembly.
        /// </summary>
        public static string VersionName;

        static Program()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(asm.Location);
            VersionName = versionInfo.ProductVersion;
        }

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
