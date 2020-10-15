using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowaria.Railnote.Curve
{
    public sealed class CurverailConfiguration
    {
        public float Quality;
        public float UVDuration;
        public float UpdateInterval;

        public static CurverailConfiguration Default()
        {
            return new CurverailConfiguration()
            {
                Quality = 1.0f,
                UVDuration = 1.0f,
                UpdateInterval = 0.05f
            };
        }
    }
}
