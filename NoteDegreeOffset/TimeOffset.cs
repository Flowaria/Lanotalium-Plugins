using EasyRequest;
using Lanotalium.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteDegreeOffset
{
    public class TimeOffset : ILanotaliumPlugin
    {
        public string Name(Language language)
        {
            return "Degree Offset (After Time)";
        }

        public string Description(Language language)
        {
            return "Add Offset to Degree after Specific Time";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            if (!context.IsProjectLoaded)
            {
                yield break;
            }

            CurrentTime.ChartTime = context.TunerManager.ChartTime;

            var request = new Request<AskTimeAndOffset>();
            yield return context.UserRequest.Request(request, "Time based Degree Offset");

            if(!request.Succeed)
            {
                yield break;
            }

            var result = request.Object;
            var tapManager = context.TunerManager.TapNoteManager;
            var holdManager = context.TunerManager.HoldNoteManager;
            var operationManager = context.OperationManager;
            foreach (var tapNote in tapManager.TapNote)
            {
                if (tapNote.Time >= result.Time)
                {
                    operationManager.SetTapNoteDegree(tapNote, tapNote.Degree + result.Degree, false, true);
                }
            }

            foreach (var holdNote in holdManager.HoldNote)
            {
                if (holdNote.Time >= result.Time)
                {
                    operationManager.SetHoldNoteDegree(holdNote, holdNote.Degree + result.Degree, false, true);
                }
            }
        }
    }

    public class AskTimeAndOffset
    {
        [Name("All note after this time will be applied")]
        public float Time = CurrentTime.ChartTime;

        [Name("Offset Degree")]
        public float Degree = 0.0f;
    }

    public static class CurrentTime
    {
        public static float ChartTime { get; set; }
    }
}
