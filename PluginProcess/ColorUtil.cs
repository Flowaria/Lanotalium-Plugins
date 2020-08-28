using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Lanotalium.Plugin.Simple
{
    public static class ColorUtil
    {
        public static Color FromInt(int r, int g, int b, int a)
        {
            float newR = (float)r / 255.0f;
            float newG = (float)g / 255.0f;
            float newB = (float)b / 255.0f;
            float newA = (float)a / 255.0f;

            return new Color(newR, newG, newB, newA);
        }
    }
}
