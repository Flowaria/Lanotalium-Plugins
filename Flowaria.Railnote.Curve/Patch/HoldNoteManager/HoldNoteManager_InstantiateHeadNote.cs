using Flowaria.Railnote.Curve.Lib;
using HarmonyLib;
using Lanotalium.Chart;

namespace Flowaria.Railnote.Curve.Patch
{
    [HarmonyPatch(typeof(LimHoldNoteManager))]
    [HarmonyPatch("InstantiateHeadNote")]
    internal class HoldNoteManager_InstantiateHeadNote
    {
        private static void Postfix(LimHoldNoteManager __instance, LanotaHoldNote Note)
        {
            var worker = Note.HoldNoteGameObject.AddComponent<HoldLineWorker>();
            worker.TouchMaterial = __instance.HoldTouch;
            worker.UnTouchMaterial = __instance.HoldUntouch;
            worker.Note = Note;
        }
    }
}