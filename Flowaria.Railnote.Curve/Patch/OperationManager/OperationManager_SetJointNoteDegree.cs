using Flowaria.Railnote.Curve.Lib;
using HarmonyLib;
using Lanotalium.Chart;

namespace Flowaria.Railnote.Curve.Patch
{
    //SetJointNoteDegree(LanotaHoldNote HoldNoteData, LanotaJoints JointNoteData, float Degree, bool isAbsolute, bool isChained, bool SaveOperation = true)
    [HarmonyPatch(typeof(LimOperationManager))]
    [HarmonyPatch("SetJointNoteDegree")]
    internal class OperationManager_SetJointNoteDegree
    {
        private static void Postfix(LanotaHoldNote HoldNoteData)
        {
            HoldNoteData.HoldNoteGameObject?.GetComponent<HoldLineWorker>()?.ForceUpdate();
        }
    }
}