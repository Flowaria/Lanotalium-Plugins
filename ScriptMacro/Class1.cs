using Lanotalium.Plugin;
using ScriptMacro.Lua;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptMacro
{
    public class Class1 : ILanotaliumPlugin
    {
        public string Description(Language language)
        {
            return "Lua Test";
        }

        public string Name(Language language)
        {
            return "Lua Test";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            new LuaEngine(context).load();
            yield return null;
        }
    }
}
