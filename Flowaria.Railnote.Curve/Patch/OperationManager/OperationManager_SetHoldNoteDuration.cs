using Flowaria.Railnote.Curve.Lib;
using HarmonyLib;
using Lanotalium.Chart;

namespace Flowaria.Railnote.Curve.Patch
{
    //SetHoldNoteDuration(LanotaHoldNote HoldNoteData, float Duration, bool SaveOperation = true)
    [HarmonyPatch(typeof(LimOperationManager))]
    [HarmonyPatch("SetHoldNoteDuration")]
    internal class OperationManager_SetHoldNoteDuration
    {
        private static void Postfix(LanotaHoldNote HoldNoteData)
        {
            HoldNoteData.HoldNoteGameObject?.GetComponent<HoldLineWorker>()?.ForceUpdate();
        }
    }
}