using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowaria.BetterHighlight
{
    [BepInPlugin("flowaria.bepinex.improve.betterhighlight", "Better Highlight", "1.0.0.0")]
    [BepInProcess("Lanotalium.exe")]
    public class Class1 : BaseUnityPlugin
    {
        private void Awake()
        {
            var harmony = new Harmony("flowaria.lanotalium.harmony.betterhighlight");
            harmony.PatchAll();
        }
    }
}
