using Lanotalium.Chart;
using Lanotalium.Plugin;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptMacro.Lua
{
    public partial class LuaCallback
    {
        private Script _Script;
        private LimOperationManager _Operation;
        private LimTunerManager _Tuner;
        private MessageBoxManager _MessageBox;

        public LuaCallback(LanotaliumContext context, Script script)
        {
            _Operation = context.OperationManager;
            _Tuner = context.TunerManager;
            _MessageBox = context.MessageBox;

            _Script = script;

            _Script.Globals["get_field_int"] = (Func<string, int>)GetRequestIntField;
            _Script.Globals["get_field_float"] = (Func<string, float>)GetRequestFloatField;
            _Script.Globals["get_field_bool"] = (Func<string, bool>)GetRequestBoolField;
            _Script.Globals["get_field_string"] = (Func<string, string>)GetRequestStringField;

            _Script.Globals["showmessage"] = (Action<string, string, string>)ShowMessage;
            _Script.Globals["showmsg"] = (Action<string, string, string>)ShowMessage;

            _Script.Globals["add_click"] = (Action<float, float>)CreateClickNote;
            _Script.Globals["add_catch"] = (Action<float, float>)CreateCatchNote;
            _Script.Globals["add_flickin"] = (Action<float, float>)CreateFlickInNote;
            _Script.Globals["add_flickout"] = (Action<float, float>)CreateFlickOutNote;

            _Script.Globals["add_linear"] = (Action<float, float, float, float, int>)CreateLinearMotion;
            _Script.Globals["add_circular"] = (Action<float, float, float, float, int>)CreateCircularMotion;
            _Script.Globals["add_vertical"] = (Action<float, float, float, int>)CreateVerticalMotion;
            _Script.Globals["add_rotation"] = (Action<float, float, float, int>)CreateRotationMotion;

            _Script.Globals["add_bpm"] = (Action<float, float>)CreateBPMChange;
            _Script.Globals["add_scroll"] = (Action<float, float>)CreateScrollChange;

            _Script.Globals["current_bpm"] = (Func<float>)GetCurrentBpm;
            _Script.Globals["current_scrollspeed"] = (Func<float>)GetCurrentScrollSpeed;
            _Script.Globals["current_speed"] = (Func<float>)GetCurrentScrollSpeed;

            _Script.Globals["current_x"] = (Func<float>)GetCurrentX;
            _Script.Globals["current_y"] = (Func<float>)GetCurrentZ;
            _Script.Globals["current_height"] = (Func<float>)GetCurrentY;
            _Script.Globals["current_rotation"] = (Func<float>)GetCurrentRotation;
            _Script.Globals["current_rho"] = (Func<float>)GetCurrentRho;
            _Script.Globals["current_theta"] = (Func<float>)GetCurrentTheta;
        }

        public void ShowMessage(string content, string okFuncName = "", string cancelFuncName = "")
        {
            MessageBoxManager.MessageBoxCallBack okCallback = null;
            MessageBoxManager.MessageBoxCallBack cancelCallback = null;

            if (!string.IsNullOrEmpty(okFuncName))
            {
                okCallback = delegate () { _Script.Call(_Script.Globals[okFuncName]); };
            }

            if (!string.IsNullOrEmpty(cancelFuncName))
            {
                cancelCallback = delegate () { _Script.Call(_Script.Globals[cancelFuncName]); };
            }

            _MessageBox.ShowMessage(content, okCallback, cancelCallback);
        }

    }
}
