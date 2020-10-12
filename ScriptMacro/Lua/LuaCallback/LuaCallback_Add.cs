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
        #region Operation - Note

        public void CreateClickNote(float timing, float degree)
        {
            CreateTapNote(0, timing, degree);
        }

        public void CreateCatchNote(float timing, float degree)
        {
            CreateTapNote(4, timing, degree);
        }

        public void CreateFlickInNote(float timing, float degree)
        {
            CreateTapNote(2, timing, degree);
        }
        public void CreateFlickOutNote(float timing, float degree)
        {
            CreateTapNote(3, timing, degree);
        }

        private void CreateTapNote(int type, float timing, float degree)
        {
            _Operation.AddTapNote(new LanotaTapNote()
            {
                Type = type,
                Time = timing,
                Degree = degree
            });
        }

        #endregion

        #region Operation - Motion

        public void CreateLinearMotion(float timing, float length = -1.0f, float rho = 0.0f, float theta = 0.0f, int ease = 0)
        {
            CreateHorizontalMotion(11, timing, length, theta, rho, ease);
        }

        public void CreateCircularMotion(float timing, float length = -1.0f, float rho = 0.0f, float theta = 0.0f, int ease = 0)
        {
            CreateHorizontalMotion(8, timing, length, theta, rho, ease);
        }

        private void CreateHorizontalMotion(int type, float timing, float length = -1.0f, float ctp0 = 0.0f, float ctp1 = 0.0f, int ease = 0)
        {
            var camera = new LanotaCameraXZ()
            {
                Time = timing,
                Duration = length,
                Type = type,
                ctp = ctp0,
                ctp1 = ctp1,
                cfmi = ease
            };
            _Operation.AddHorizontal(camera, length <= 0.0f);
        }

        public void CreateVerticalMotion(float timing, float length, float ctp0 = 0.0f, int ease = 0)
        {
            var camera = new LanotaCameraY()
            {
                Time = timing,
                Duration = length,
                Type = 10,
                ctp = ctp0,
                cfmi = ease
            };
            _Operation.AddVertical(camera, length <= 0.0f);
        }

        public void CreateRotationMotion(float timing, float length, float ctp0 = 0.0f, int ease = 0)
        {
            var camera = new LanotaCameraRot()
            {
                Time = timing,
                Duration = length,
                Type = 13,
                ctp = ctp0,
                cfmi = ease
            };
            _Operation.AddRotation(camera, length <= 0.0f);
        }

        public void CreateBPMChange(float timing, float bpm)
        {
            var bpmchange = new LanotaChangeBpm()
            {
                Time = timing,
                Bpm = bpm
            };
            _Operation.AddBpm(bpmchange);
        }

        public void CreateScrollChange(float timing, float speed)
        {
            var speedchange = new LanotaScroll()
            {
                Time = timing,
                Speed = speed
            };
            _Operation.AddScrollSpeed(speedchange);
        }

        #endregion
    }
}
