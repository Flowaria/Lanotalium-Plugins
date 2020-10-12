using Flowaria.Railnote.Curve.Lib;
using HarmonyLib;
using Lanotalium.Chart;

namespace Flowaria.Railnote.Curve.Patch
{
    //RefreshJointAbsoluteValues(LanotaHoldNote HoldNoteData)
    [HarmonyPatch(typeof(LimOperationManager))]
    [HarmonyPatch("RefreshJointAbsoluteValues")]
    internal class OperationManager_RefreshJointAbsoluteValues
    {
        private static void Postfix(LanotaHoldNote HoldNoteData)
        {
            HoldNoteData.HoldNoteGameObject?.GetComponent<HoldLineWorker>()?.ForceUpdate();
        }
    }
}