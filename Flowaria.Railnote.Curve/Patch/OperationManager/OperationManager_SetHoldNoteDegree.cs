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
    //SetHoldNoteDegree(LanotaHoldNote HoldNoteData, float Degree, bool isAbsolute, bool SaveOperation = true)
    [HarmonyPatch(typeof(LimOperationManager))]
    [HarmonyPatch("SetHoldNoteDegree")]
    class OperationManager_SetHoldNoteDegree
    {
        static void Postfix(LanotaHoldNote HoldNoteData)
        {
            HoldNoteData.HoldNoteGameObject?.GetComponent<HoldLineWorker>()?.ForceUpdate();
        }
    }
}
