using HarmonyLib;
using UnityEngine;

namespace Flowaria.Railnote.Curve.Patch
{
    [HarmonyPatch(typeof(LimHoldNoteManager))]
    [HarmonyPatch("Initialize")]
    internal static class HoldNoteManager_Initialize
    {
        private static void Prefix(LimHoldNoteManager __instance)
        {
            var bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/Assets/curvedrail");
            if (bundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                return;
            }

            __instance.HoldTouch = bundle.LoadAsset<Material>("Rail_Pressed");
            __instance.HoldUntouch = bundle.LoadAsset<Material>("Rail_Unpressed");

            bundle.Unload(false);
        }
    }
}