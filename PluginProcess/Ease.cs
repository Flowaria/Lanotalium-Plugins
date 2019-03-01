using System;
using System.Collections.Generic;
using System.Text;

namespace Lanotalium.Plugin.Simple
{
    public class Ease
    {
        public static float CalculateEase(float Percent, int Mode)
        {
            if (Percent >= 1.0) return 1.0f;
            else if (Percent <= 0.0) return 0.0f;
            switch (Mode)
            {
                //None
                case 0:
                    return Percent;

                //QuarticEase
                case 1:
                    return Percent * Percent * Percent * Percent;
                case 2:
                    return -(Percent - 1) * (Percent - 1) * (Percent - 1) * (Percent - 1) + 1;

                case 3:
                    return (Percent < 0.5) ?
                        (Percent * Percent * Percent * Percent * 8) :
                        ((Percent - 1) * (Percent - 1) * (Percent - 1) * (Percent - 1) * -8 + 1);

                //CubicEase
                case 4:
                    return Percent * Percent * Percent;
                case 5:
                    return (Percent - 1) * (Percent - 1) * (Percent - 1) + 1;
                case 6:
                    return (Percent < 0.5) ?
                        (Percent * Percent * Percent * 4) :
                        ((Percent - 1) * (Percent - 1) * (Percent - 1) * 4 + 1);

                //ExponentialEase
                case 7:
                    return (float)Math.Pow(2, 10 * (Percent - 1));
                case 8:
                    return (float)-Math.Pow(2.0f, -10 * Percent) + 1;
                case 9:
                    return (Percent < 0.5) ?
                        (float)(Math.Pow(2, 10 * (2 * (float)Percent - 1)) / 2) :
                        (float)((-Math.Pow(2, -10 * (2 * Percent - 1)) + 2) / 2);

                //SineEase
                case 10:
                    return (float)-Math.Cos(Percent * Math.PI / 2) + 1;
                case 11:
                    return (float)Math.Sin(Percent * Math.PI / 2);
                case 12:
                    return (float)(Math.Cos(Percent * Math.PI) - 1) / -2;
            }
            return 1;
        }
    }
}
