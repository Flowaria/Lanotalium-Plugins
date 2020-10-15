using BepInEx;
using Flowaria.Railnote.Curve.Lib;
using HarmonyLib;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Flowaria.Railnote.Curve
{
    using Cfg = CurverailConfiguration;

    [BepInPlugin("flowaria.bepinex.railnote.curve", "Curved Rail note", "1.0.0.0")]
    [BepInProcess("Lanotalium.exe")]
    public class Entry : BaseUnityPlugin
    {
        private void Awake()
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configFile = basePath + "\\curverail.json";

            if(File.Exists(configFile))
            {
                var config = JsonConvert.DeserializeObject<Cfg>(File.ReadAllText(configFile));
                HoldLineWorker.QUALITY = Mathf.Clamp(config.Quality, 0.075f, 5.0f);
                HoldLineWorker.UV_DURATION = Mathf.Clamp(config.UVDuration, 0.001f, 999.0f);
                HoldLineWorker.UPDATE_INTERVAL = Mathf.Clamp(config.UpdateInterval, 0.016f, 5.0f);
            }
            else
            {
                File.WriteAllText(configFile, JsonConvert.SerializeObject(Cfg.Default()));
            }

            var harmony = new Harmony("flowaria.lanotalium.harmony.railnotecurve");
            harmony.PatchAll();
        }
    }
}