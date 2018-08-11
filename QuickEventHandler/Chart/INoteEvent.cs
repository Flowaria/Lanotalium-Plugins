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
    public interface INoteEvent
    {
        IEnumerator OnTabNoteEnabled(LanotaliumContext context, LanotaTapNote note);
        IEnumerator OnTabNoteDisabled(LanotaliumContext context, LanotaTapNote note);
        IEnumerator OnTapNoteClicked(LanotaliumContext context, LanotaTapNote note);

        IEnumerator OnHoldNoteEnabled(LanotaliumContext context, LanotaHoldNote note);
        IEnumerator OnHoldNoteDisabled(LanotaliumContext context, LanotaHoldNote note);
        IEnumerator OnHoldNoteClicked(LanotaliumContext context, LanotaHoldNote note);
        IEnumerator OnHoldNoteClickedMultiple(LanotaliumContext context, LanotaHoldNote note);
    }
}
