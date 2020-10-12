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
    //SetJointNoteEase(LanotaJoints JointNoteData, int Ease, bool SaveOperation = true)
    [HarmonyPatch(typeof(LimOperationManager))]
    [HarmonyPatch("SetJointNoteEase")]
    class OperationManager_SetJointNoteEase
    {
        static void Postfix(LanotaJoints JointNoteData)
        {
            var holdManager = LimTunerManager.Instance.HoldNoteManager;
            foreach (var hold in holdManager.HoldNote)
            {
                if(hold.Time > JointNoteData.aTime)
                {
                    continue;
                }

                if(hold.Jcount == 0)
                {
                    continue;
                }

                if(hold.Joints == null)
                {
                    continue;
                }

                if(hold.Joints.Any(x=> x == JointNoteData))
                {
                    hold.HoldNoteGameObject?.GetComponent<HoldLineWorker>()?.ForceUpdate();
                }
            }
        }
    }
}
