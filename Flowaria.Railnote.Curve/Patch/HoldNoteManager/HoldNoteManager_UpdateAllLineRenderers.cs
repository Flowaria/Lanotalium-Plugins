using HarmonyLib;

namespace Flowaria.Railnote.Curve.Patch
{
    [HarmonyPatch(typeof(LimHoldNoteManager))]
    [HarmonyPatch("UpdateAllLineRenderers")]
    internal class HoldNoteManager_UpdateAllLineRenderers
    {
        private static bool Prefix()
        {
            return false;
        }
    }
}