using Flowaria.Railnote.Curve.Lib;
using HarmonyLib;
using Lanotalium.Chart;

namespace Flowaria.Railnote.Curve.Patch
{
    //SetHoldNoteDegree(LanotaHoldNote HoldNoteData, float Degree, bool isAbsolute, bool SaveOperation = true)
    [HarmonyPatch(typeof(LimOperationManager))]
    [HarmonyPatch("SetHoldNoteDegree")]
    internal class OperationManager_SetHoldNoteDegree
    {
        private static void Postfix(LanotaHoldNote HoldNoteData)
        {
            HoldNoteData.HoldNoteGameObject?.GetComponent<HoldLineWorker>()?.ForceUpdate();
        }
    }
}