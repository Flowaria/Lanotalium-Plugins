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
    public class CreateLinear : ILanotaliumPlugin
    {
        public string Name(Language language)
        {
            return "Create Linear Motion (XY)";
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

            var request = new Request<AskForLinear>();
            yield return context.UserRequest.Request(request, "Linear Motion Creation");
            if (!request.Succeed)
            {
                yield break;
            }

            var result = request.Object;
            var polar = CoordMath.ToPolar(new Vector2(result.X, result.Y));

            //8 cir 11 linear
            //0 deg 1 radius

            var camera = new LanotaCameraXZ()
            {
                Time = context.TunerManager.ChartTime,
                Type = 11,
                ctp = polar.y,
                ctp1 = polar.x
            };
            context.OperationManager.AddHorizontal(camera);

            yield return null;
        }
    }

    public class AskForLinear
    {
        [Name("X-coordinate")]
        public float X;

        [Name("Y-coordinate")]
        public float Y;
    }
}
