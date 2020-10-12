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
    [HarmonyPatch(typeof(LimOperationManager), "SetHoldNoteTime")]
    public static class OperationManager_SetHoldNoteTime
    {
        static void Postfix(LanotaHoldNote HoldNoteData, float Time, bool SaveOperation)
        {
            if (HoldNoteData.HoldNoteGameObject != null)
            {
                var sort = HoldNoteData.HoldNoteGameObject.GetComponent<SortingGroup>();
                if (sort != null)
                {
                    sort.sortingOrder = -(int)(HoldNoteData.Time * 10000.0f);
                }
            }
        }
    }
}
