using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Flowaria.BetterHighlight.Patch
{
    public struct TimeIDPair
    {
        public static readonly int HOLDFLAG = 1 << 31;

        public float Time;
        public int Index;
        public bool IsHold;

        public TimeIDPair(float time, int index, bool hold)
        {
            Time = time;
            Index = index;
            IsHold = hold;
        }
    }


    [HarmonyPatch(typeof(LimCreatorManager), "AutoHighlight")]
    public static class CreatorManager_AutoHighlight
    {
        static bool Prefix()
        {
            Debug.Log("using fixed type autohighlight");

            var tunerManager = LimTunerManager.Instance;
            var operationManager = LimOperationManager.Instance;

            List<TimeIDPair> timeIDPairs = new List<TimeIDPair>();

            var TapNotes = tunerManager.TapNoteManager.TapNote;
            for (int i = 0;i< TapNotes.Count; i++)
            {
                var tapNote = TapNotes[i];
                timeIDPairs.Add(new TimeIDPair(tapNote.Time, i, false));
            }

            var HoldNotes = tunerManager.HoldNoteManager.HoldNote;
            for (int i = 0; i < HoldNotes.Count; i++)
            {
                var holdNote = HoldNotes[i];
                timeIDPairs.Add(new TimeIDPair(holdNote.Time, i, true));
            }

            var newPairs = timeIDPairs.OrderBy(o => o.Time).ToArray();

            if(newPairs.Length < 2)
            {
                return false;
            }

            TimeIDPair lastPair = newPairs[0];
            for(int i = 1;i < newPairs.Length;i++)
            {
                var pair = newPairs[i];

                if(IsApproximate(pair.Time, lastPair.Time, 0.0015f))
                {
                    if(pair.IsHold)
                    {
                        operationManager.SetHoldNoteCombination(HoldNotes[pair.Index], true);
                    }
                    else
                    {
                        operationManager.SetTapNoteCombination(TapNotes[pair.Index], true);
                    }

                    if(lastPair.IsHold)
                    {
                        operationManager.SetHoldNoteCombination(HoldNotes[lastPair.Index], true);
                        
                    }
                    else
                    {
                        operationManager.SetTapNoteCombination(TapNotes[lastPair.Index], true);
                    }
                }
                else
                {
                    if (pair.IsHold)
                    {
                        operationManager.SetHoldNoteCombination(HoldNotes[pair.Index], false);
                    }
                    else
                    {
                        operationManager.SetTapNoteCombination(TapNotes[pair.Index], false);
                    }
                }
                lastPair = pair;
            }

            return false;
        }

        public static bool IsApproximate(float value1, float value2, float allowedDelta)
        {
            return (value1 - allowedDelta) <= value2 && value2 <= (value1 + allowedDelta);
        }
    }
}
