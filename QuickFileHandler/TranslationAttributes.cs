using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lanotalium.Plugin.Files
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FileNameAttribute : Attribute
    {
        public string FileName;

        public FileNameAttribute(string name)
        {
            FileName = name;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class NodeNameAttribute : Attribute
    {
        public string NodeName;

        public NodeNameAttribute(string name)
        {
            NodeName = name.Replace("$", "");
        }
    }
}
