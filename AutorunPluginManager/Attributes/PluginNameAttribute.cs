using System;

namespace Flowaria.AutorunPlugin
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PluginNameAttribute : Attribute
    {
        public string Name { get; private set; }

        public PluginNameAttribute(string name)
        {
            Name = name;
        }
    }
}