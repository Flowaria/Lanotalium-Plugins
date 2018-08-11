using Lanotalium.Chart;
using Lanotalium.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lanotalium.Plugin.Events.Chart
{
    public interface IHorizontalMotionEvent
    {
        IEnumerator OnHorizontalStarted(LanotaliumContext context, LanotaCameraXZ motion);
        IEnumerator OnHorizontalEnded(LanotaliumContext context, LanotaCameraXZ motion);
    }

    public interface IVerticalMotionEvent
    {
        IEnumerator OnVerticalStarted(LanotaliumContext context, LanotaCameraY motion);
        IEnumerator OnVerticalEnded(LanotaliumContext context, LanotaCameraY motion);
    }

    public interface IRotationMotionEvent
    {
        IEnumerator OnRotationStarted(LanotaliumContext context, LanotaCameraRot motion);
        IEnumerator OnRotationEnded(LanotaliumContext context, LanotaCameraRot motion);
    }
}
