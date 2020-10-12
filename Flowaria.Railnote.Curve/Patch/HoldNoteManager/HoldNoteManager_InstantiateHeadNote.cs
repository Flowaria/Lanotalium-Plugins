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
    [HarmonyPatch(typeof(LimHoldNoteManager))]
    [HarmonyPatch("InstantiateHeadNote")]
    class HoldNoteManager_InstantiateHeadNote
    {
        static void Postfix(LimHoldNoteManager __instance, LanotaHoldNote Note)
        {
            var worker = Note.HoldNoteGameObject.AddComponent<HoldLineWorker>();
            worker.TouchMaterial = __instance.HoldTouch;
            worker.UnTouchMaterial = __instance.HoldUntouch;
            worker.Note = Note;
        }
    }
}
