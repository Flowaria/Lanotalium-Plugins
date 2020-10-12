using Flowaria.Railnote.Curve.Lib;
using HarmonyLib;
using Lanotalium.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowaria.Railnote.Curve.Patch
{
    //SetHoldNoteDuration(LanotaHoldNote HoldNoteData, float Duration, bool SaveOperation = true)
    [HarmonyPatch(typeof(LimOperationManager))]
    [HarmonyPatch("SetHoldNoteDuration")]
    class OperationManager_SetHoldNoteDuration
    {
        static void Postfix(LanotaHoldNote HoldNoteData)
        {
            HoldNoteData.HoldNoteGameObject?.GetComponent<HoldLineWorker>()?.ForceUpdate();
        }
    }
}
