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
    [HarmonyPatch(typeof(LimOperationManager), "SetTapNoteTime")]
    public static class OperationManager_SetTapNoteTime
    {

        static void Postfix(LanotaTapNote TapNoteData, float Time, bool SaveOperation)
        {
            if (TapNoteData.TapNoteGameObject != null)
            {
                var sort = TapNoteData.TapNoteGameObject.GetComponent<SortingGroup>();
                if(sort != null)
                {
                    sort.sortingOrder = -(int)(TapNoteData.Time * 10000.0f);
                }
            }
        }
    }
}
