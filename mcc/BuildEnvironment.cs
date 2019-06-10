using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace mcc
{
    public class BuildEnvironment
    {
        /// <summary>
        /// Path to datapack.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Path to output
        /// </summary>
        public string OutputPath { get; set; }

        public Dictionary<string, string> Constants = new Dictionary<string, string>();

        public BuildEnvironment(string path)
        {
            Path = path;

            Constants.Add("globals_scoreboard", "globals");
            Constants.Add("compiled_header", "Compiled {file} at {date} by McFuncCompiler");
            Constants.Add("mcfunc_compiler_version", Program.VersionName);
        }

        /// <summary>
        /// Get path to file from a Minecraft-style namespaced resource ID.
        /// </summary>
        /// <param name="id">Resource ID</param>
        /// <param name="extension">File extension to assume if none is given.</param>
        /// <param name="rootPath">The root path of the datapack. If null the one specified in the BuildEnvironment will be used.</param>
        /// <returns></returns>
        public string GetPath(string id, string extension, string rootPath = null)
        {
            extension = extension.TrimStart('.');
            rootPath = rootPath ?? Path;

            // Split ID into namespace and path
            Match match = Regex.Match(id, @"^([\w]*):([^:]+)$");
            string funcNamespace = match.Groups[1].Value;
            string funcPath = match.Groups[2].Value;

            string funcFileName = funcPath.Substring(Math.Max(funcPath.LastIndexOf('/'), 0));
            string funcExtension = null;
            if (funcFileName.Contains('.'))
                funcExtension = funcPath.Substring(funcFileName.LastIndexOf('.'));

            if (funcExtension == null)
            {
                // Add extension if there is none
                if (!funcPath.EndsWith("." + extension))
                    funcPath += "." + extension;
            }

            return $"{rootPath.Replace('\\', '/').TrimEnd('/')}/data/{funcNamespace}/functions/{funcPath}";
        }
    }
}
