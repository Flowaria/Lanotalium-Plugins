using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XYMotion
{
    public static class CoordMath
    {
        public static Vector2 ToPolar(Vector2 xycoord)
        {
            var roh = Mathf.Sqrt(xycoord.x * xycoord.x + xycoord.y * xycoord.y);
            var theta = 180.0f - Mathf.Atan2(xycoord.y, xycoord.x) * Mathf.Rad2Deg;
            return new Vector2(roh, theta);


        }
    }
}
