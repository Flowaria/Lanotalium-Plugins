using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Flowaria.Railnote.Curve.Lib
{
    public struct MeshPoint
    {
        public Vector3 BasePoint;

        public Vector3 GetLeft(float percent)
        {
            var width = CalculateEasedCurve(percent);
            width *= 5.65f;

            return Quaternion.Euler(0.0f, -width, 0.0f) * BasePoint;
        }

        public Vector3 GetRight(float percent)
        {
            var width = CalculateEasedCurve(percent);
            width *= 5.65f;

            return Quaternion.Euler(0.0f, +width, 0.0f) * BasePoint;
        }

        private float CalculateEasedCurve(float Percent)
        {
            return EasingLookupTable.EaseEvaluate(Percent, 8);
        }
    }
}
