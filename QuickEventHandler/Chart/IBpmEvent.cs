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
    public interface IBpmEvent
    {
        IEnumerator OnBpmChanged(LanotaliumContext context, LanotaChangeBpm bpm);
    }
}
