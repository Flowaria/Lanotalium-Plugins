using Lanotalium.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoreWaveform
{
    public class Class1 : ILanotaliumPlugin
    {
        public string Description(Language language)
        {
            return "Restore Waveform";
        }

        public string Name(Language language)
        {
            return "Restore Waveform";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            if(context.IsProjectLoaded)
            {
                context.EditorManager.TimeLineWindow.WaveformManager.OnMusicLoaded();
            }

            yield return null;
        }
    }
}
