using EasyRequest;
using Lanotalium.Chart;
using Lanotalium.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XYMotion
{
    public class CreateCircular : ILanotaliumPlugin
    {
        public string Name(Language language)
        {
            return "Create Circular Motion (XY)";
        }

        public string Description(Language language)
        {
            return "";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            if (!context.IsProjectLoaded)
            {
                yield break;
            }

            var request = new Request<AskForCircular>();
            yield return context.UserRequest.Request(request, "Circular Motion Creation");
            if (!request.Succeed)
            {
                yield break;
            }

            var result = request.Object;
            var newPolar = CoordMath.ToPolar(new Vector2(result.X, result.Y));

            var camManager = context.TunerManager.CameraManager;
            var currentPolar = new Vector2(camManager.CurrentRou, camManager.CurrentTheta);
            var deltaPolar = Vector2.zero;

            //invert roh
            if(currentPolar.x < 0.0f)
            {
                currentPolar.x *= -1.0f;
                currentPolar.y = (currentPolar.y + 180.0f) % 360.0f;
            }

            deltaPolar.x = newPolar.x - currentPolar.x;
            deltaPolar.y = newPolar.y - currentPolar.y;

            //theta is negative (clockwise)
            //and it's anti-clockwise
            if(deltaPolar.y < 0.0f && !result.Clockwise)
            {
                deltaPolar.y = 360.0f + deltaPolar.y;
            }
            //theta is positive (anti-clockwise)
            //and it's clockwise
            else if(deltaPolar.y >= 0.0f && result.Clockwise)
            {
                deltaPolar.y = -(360.0f - deltaPolar.y);
            }

            //add revolution
            if(result.Revolution > 0)
            {
                if(result.Clockwise)
                {
                    deltaPolar.y -= 360.0f * result.Revolution;
                }
                else
                {
                    deltaPolar.y += 360.0f * result.Revolution;
                }
            }

            //8 cir 11 linear
            //0 deg 1 radius
            var camera = new LanotaCameraXZ()
            {
                Time = context.TunerManager.ChartTime,
                Type = 8,
                ctp = deltaPolar.y,
                ctp1 = deltaPolar.x
            };
            context.OperationManager.AddHorizontal(camera);

            yield return null;
        }
    }

    public class AskForCircular
    {
        [Name("X-coordinate")]
        public float X;

        [Name("Y-coordinate")]
        public float Y;

        [Name("Revolution Count")]
        [Range(0, 999999999)]
        public int Revolution = 0;

        [Name("Clockwise")]
        public bool Clockwise = true;
    }
}
