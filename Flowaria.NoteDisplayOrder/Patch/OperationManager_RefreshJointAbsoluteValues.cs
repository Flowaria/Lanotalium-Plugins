using HarmonyLib;
using Lanotalium.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace Flowaria.NoteDisplayOrder.Patch
{
    [HarmonyPatch(typeof(LimOperationManager), "RefreshJointAbsoluteValues")]
    public static class OperationManager_RefreshJointAbsoluteValues
    {
        static void Postfix(LanotaHoldNote HoldNoteData)
        {
            if(HoldNoteData.Joints == null)
            {
                return;
            }

            for(int i = 0;i<HoldNoteData.Joints.Count;i++)
            {
                var joint = HoldNoteData.Joints[i];

                if (joint.JointGameObject != null)
                {
                    var sort = joint.JointGameObject.GetComponent<SortingGroup>();
                    if (sort == null)
                    {
                        sort = joint.JointGameObject.AddComponent<SortingGroup>();
                        sort.sortingLayerName = "Note";
                    }

                    sort.sortingOrder = -(int)(joint.aTime * 10000.0f);
                    Debug.Log(sort.sortingOrder);
                }
            }
        }
    }
}
