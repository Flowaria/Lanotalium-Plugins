using Lanotalium.Chart;
using Lanotalium.Plugin;
using Lanotalium.Plugin.Events.Chart;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickEventHandler_Example
{
    public class Class1 : IBpmEvent
    {
        public IEnumerator OnBpmChanged(LanotaliumContext context, LanotaChangeBpm bpm)
        {
            context.MessageBox.ShowMessage(String.Format("BPM JUST CHANGED! {0}:{1}", bpm.Time, bpm.Bpm));
            yield return null;
        }
    }
}
