using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFuncCompiler
{
    public class BuildEnvironment
    {
        public string Path { get; set; }

        public Dictionary<string, string> Constants = new Dictionary<string, string>();

        public BuildEnvironment(string path)
        {
            Path = path;
        }
    }
}
