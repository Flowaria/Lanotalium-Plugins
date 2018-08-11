using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lanotalium.Plugin.Events.Chart
{
    public interface IBeatlineEvent
    {
        IEnumerator OnTouchedBeatline(LanotaliumContext context);
    }
}
