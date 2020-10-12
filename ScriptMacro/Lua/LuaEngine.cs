using Lanotalium.Plugin;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptMacro.Lua
{
    public class LuaEngine
    {
        private Script _Script;
        private LuaCallback _Callback;

        public LuaEngine(LanotaliumContext context)
        {
            _Script = new Script();
            _Callback = new LuaCallback(context, _Script);
        }

        public void load()
        {
            string scriptCode = @"    
            -- defines a factorial function
            function start ()
                printMessage('Hello', 'onPressOk', 'okPressCancel')
            end
            
            function onPressOk()
                printMessage('Ok')
            end

            function onPressCancel()
                printMessage('Cancel')
            end
";
            _Script.DoString(scriptCode);
            _Script.Call(_Script.Globals["start"]);
        }

        
    }
}
