using HarmonyLib;

namespace Flowaria.Railnote.Curve.Patch
{
    [HarmonyPatch(typeof(LimHoldNoteManager))]
    [HarmonyPatch("UpdateAllLineMaterial")]
    internal class HoldNoteManager_UpdateAllLineMaterial
    {
        private static bool Prefix()
        {
            return false;
        }
    }
}