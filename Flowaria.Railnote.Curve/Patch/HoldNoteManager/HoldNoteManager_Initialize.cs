using Flowaria.Railnote.Curve.Lib;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Flowaria.Railnote.Curve.Patch
{
    [HarmonyPatch(typeof(LimHoldNoteManager))]
    [HarmonyPatch("Initialize")]
    static class HoldNoteManager_Initialize
    {
        static void Prefix(LimHoldNoteManager __instance)
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
