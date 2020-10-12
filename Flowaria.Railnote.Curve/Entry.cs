using BepInEx;
using BepInEx.Harmony;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Flowaria.Railnote.Curve
{
    [BepInPlugin("flowaria.bepinex.railnote.curve", "Curved Rail note", "1.0.0.0")]
    [BepInProcess("Lanotalium.exe")]
    public class Entry : BaseUnityPlugin
    {
        private void Awake()
        {
            var harmony = new Harmony("flowaria.lanotalium.harmony.railnotecurve");
            harmony.PatchAll();
        }
    }
}
