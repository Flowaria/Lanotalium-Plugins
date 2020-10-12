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

    //SetHoldNoteTime(LanotaHoldNote HoldNoteData, float Time, bool SaveOperation = true)
    [HarmonyPatch(typeof(LimOperationManager))]
    [HarmonyPatch("SetHoldNoteTime")]
    class OperationManager_SetHoldNoteTime
    {
        static void Postfix(LanotaHoldNote HoldNoteData)
        {
            HoldNoteData.HoldNoteGameObject?.GetComponent<HoldLineWorker>()?.ForceUpdate();
        }
    }
}
