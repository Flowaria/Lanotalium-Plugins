using Flowaria.Railnote.Curve.Lib;
using HarmonyLib;
using Lanotalium.Chart;

namespace Flowaria.Railnote.Curve.Patch
{
    //SetHoldNoteTime(LanotaHoldNote HoldNoteData, float Time, bool SaveOperation = true)
    [HarmonyPatch(typeof(LimOperationManager))]
    [HarmonyPatch("SetHoldNoteTime")]
    internal class OperationManager_SetHoldNoteTime
    {
        private static void Postfix(LanotaHoldNote HoldNoteData)
        {
            HoldNoteData.HoldNoteGameObject?.GetComponent<HoldLineWorker>()?.ForceUpdate();
        }
    }
}