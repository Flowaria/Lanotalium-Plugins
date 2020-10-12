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
    [HarmonyPatch(typeof(LimTapNoteManager), "InstantiateNote")]
    public static class TapNoteManager_InstantiateNote
    {
        static void Postfix(LanotaTapNote Note)
        {
            if (Note.TapNoteGameObject != null)
            {
                var sort = Note.TapNoteGameObject.AddComponent<SortingGroup>();
                sort.sortingLayerName = "Note";
                sort.sortingOrder = -(int)(Note.Time * 10000.0f);
            }
        }
    }
}
