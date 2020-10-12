using BepInEx;
using HarmonyLib;

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