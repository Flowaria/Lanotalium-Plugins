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
    [HarmonyPatch(typeof(LimHoldNoteManager), "InstantiateHeadNote")]
    public static class HoldNoteManager_InstantiateHeadNote
    {

        static void Postfix(LanotaHoldNote Note)
        {
            if(Note.HoldNoteGameObject != null)
            {
                var sort = Note.HoldNoteGameObject.AddComponent<SortingGroup>();
                sort.sortingLayerName = "Note";
                sort.sortingOrder = -(int)(Note.Time * 10000.0f);
            }
        }
    }
}
