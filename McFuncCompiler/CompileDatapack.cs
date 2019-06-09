using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using McFuncCompiler.Util;

namespace McFuncCompiler
{
    public static class CompileDatapack
    {
        public static void Compile(Options.CompileDatapack options)
        {
            Logger.Info($"Compile datapack\n\t{options.SourceDirectory} -> {options.OutputDirectory}");

            var env = new BuildEnvironment(options.SourceDirectory);
            env.OutputPath = options.OutputDirectory;

            // Try to add custom constants into build environment
            var constants = options.ToConstantsDictonary();
            if (constants != null)
            {
                foreach (var constant in constants)
                    env.Constants.Add(constant.Key, constant.Value);
                Logger.Info($"{constants.Count} command line constant{(constants.Count == 1 ? "" : "s")} provided.");
            }

            // Get file list
            List<string> files = GetFiles(env);
            Logger.Info($"{files.Count} files found to be compiled.");

            // Start parse & compile
            Logger.Info("Starting parse & compile...");
            var parser = new Parser.Parser(env);

            Timer.Start("parse_and_compile");

            var totalParseTime = new TimeSpan();
            var totalCompileTime = new TimeSpan();

            foreach (string file in files)
            {
                // Parse
                Logger.Info($"Parsing {file}...");
                Timer.Start("parse");
                McFunction mcFunction = parser.Parse(file);
                totalParseTime = totalParseTime.Add(Timer.End("parse"));

                Logger.Debug($"Parsed {file}. {mcFunction.Commands.Count} commands found.");

                // Compile
                Logger.Info($"Compiling {file}...");
                Timer.Start("compile");
                mcFunction.Compile(env);
                totalCompileTime = totalCompileTime.Add(Timer.End("compile"));
                Logger.Debug($"Compiled {file}.");

                // Save
                mcFunction.Save(env);
            }

            TimeSpan parseAndCompileTime = Timer.End("parse_and_compile");

            Logger.Info($"Datapack compile finished. Took {Math.Round(parseAndCompileTime.TotalSeconds, 3)}s");
            Logger.Debug($"Total parse time: {Math.Round(totalParseTime.TotalSeconds, 3)}s");
            Logger.Debug($"Total compile time: {Math.Round(totalCompileTime.TotalSeconds, 3)}s");
        }

        /// <summary>
        /// Get a list of files that need to be compiled.
        /// </summary>
        private static List<string> GetFiles(BuildEnvironment env)
        {
            var paths = Directory.GetFiles(env.Path, "*.mcfunction", SearchOption.AllDirectories);

            var list = new List<string>();

            foreach (string path in paths)
            {
                string localPath = path.Substring(env.Path.Length).Replace('\\', '/');

                Match match = Regex.Match(localPath, @"^\/data\/([^\/?<>\\:*|""]+)\/functions\/([^?<>\\:*|""]+)\.mcfunction$");
                if (!match.Success)
                    continue;

                list.Add(match.Groups[1].Value + ":" + match.Groups[2].Value);
            }

            return list;
        }
    }
}
