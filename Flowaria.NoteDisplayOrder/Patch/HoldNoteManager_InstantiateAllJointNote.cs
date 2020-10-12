using HarmonyLib;
using Lanotalium.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering;

namespace Flowaria.NoteDisplayOrder.Patch
{
    [HarmonyPatch(typeof(LimHoldNoteManager), "InstantiateAllJointNote")]
    public static class HoldNoteManager_InstantiateAllJointNote
    {
        static void Postfix(LanotaHoldNote Note)
        {
            if (Note.Joints == null)
            {
                return;
            }

            for (int i = 0; i < Note.Joints.Count; i++)
            {
                var joint = Note.Joints[i];

                if (joint.JointGameObject != null)
                {
                    var sort = joint.JointGameObject.GetComponent<SortingGroup>();
                    if (sort == null)
                    {
                        sort = joint.JointGameObject.AddComponent<SortingGroup>();
                        sort.sortingLayerName = "Note";
                    }
                    //joint.dTime;

                    sort.sortingOrder = -(int)(joint.aTime * 10000.0f);
                }
            }
        }
    }
}
