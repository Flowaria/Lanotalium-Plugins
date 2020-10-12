using Lanotalium.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptMacro.Lua
{
    public partial class LuaCallback
    {
        public float GetCurrentBpm()
        {
            return _Tuner.BpmManager.CurrentBpm;
        }

        public float GetCurrentScrollSpeed()
        {
            return _Tuner.ScrollManager.CurrentScrollSpeed;
        }

        public float GetCurrentX()
        {
            return _Tuner.CameraManager.CurrentHorizontalX;
        }

        public float GetCurrentZ()
        {
            return _Tuner.CameraManager.CurrentHorizontalZ;
        }

        public float GetCurrentY()
        {
            return _Tuner.CameraManager.CurrentVerticalY;
        }

        public float GetCurrentRho()
        {
            return _Tuner.CameraManager.CurrentRou;
        }

        public float GetCurrentTheta()
        {
            return _Tuner.CameraManager.CurrentTheta;
        }

        public float GetCurrentRotation()
        {
            return _Tuner.CameraManager.CurrentRotation;
        }
    }
}
