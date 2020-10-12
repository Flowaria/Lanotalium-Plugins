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
    //RefreshJointAbsoluteValues(LanotaHoldNote HoldNoteData)
    [HarmonyPatch(typeof(LimOperationManager))]
    [HarmonyPatch("RefreshJointAbsoluteValues")]
    class OperationManager_RefreshJointAbsoluteValues
    {
        static void Postfix(LanotaHoldNote HoldNoteData)
        {
            HoldNoteData.HoldNoteGameObject?.GetComponent<HoldLineWorker>()?.ForceUpdate();
        }
    }
}
