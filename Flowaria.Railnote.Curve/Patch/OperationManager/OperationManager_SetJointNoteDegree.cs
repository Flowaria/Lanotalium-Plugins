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
    //SetJointNoteDegree(LanotaHoldNote HoldNoteData, LanotaJoints JointNoteData, float Degree, bool isAbsolute, bool isChained, bool SaveOperation = true)
    [HarmonyPatch(typeof(LimOperationManager))]
    [HarmonyPatch("SetJointNoteDegree")]
    class OperationManager_SetJointNoteDegree
    {
        static void Postfix(LanotaHoldNote HoldNoteData)
        {
            HoldNoteData.HoldNoteGameObject?.GetComponent<HoldLineWorker>()?.ForceUpdate();
        }
    }
}
