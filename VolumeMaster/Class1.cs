using EasyRequest;
using Flowaria.AutorunPlugin;
using Lanotalium.Plugin;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VolumeMaster
{
    public class VolumeContext
    {
        [Name("Click note Volume (default: 0.2)")]
        [Range(0.0f, 1.0f)]
        public float voClick = VolumeData.VoClick;

        [Name("Flick note Volume (default: 0.2)")]
        [Range(0.0f, 1.0f)]
        public float voFlick = VolumeData.VoFlick;

        [Name("Rail note Volume (default: 0.2)")]
        [Range(0.0f, 1.0f)]
        public float voRail = VolumeData.VoRail;

        [Name("Music Volume (default: 0.4)")]
        [Range(0.0f, 1.0f)]
        public float voMusic = VolumeData.VoMusic;
    }

    public class AutorunConfig
    {
        public float voClick = 0.2f;
        public float voFlick = 0.2f;
        public float voRail = 0.2f;
        public float voMusic = 0.4f;
    }

    public class Class1 : ILanotaliumPlugin
    {
        public string Description(Language language)
        {
            return "Change volume";
        }

        public string Name(Language language)
        {
            return "Volume";
        }

        [PluginName("VolumeMaster")]
        public IEnumerator Autorun_Awake(string configPath, LanotaliumContext context)
        {
            if (File.Exists(configPath))
            {
                try
                {
                    var content = File.ReadAllText(configPath);
                    var config = JsonConvert.DeserializeObject<AutorunConfig>(content);
                    if (config != null)
                    {
                        VolumeData.VoClick = config.voClick;
                        VolumeData.VoFlick = config.voFlick;
                        VolumeData.VoRail = config.voRail;
                        VolumeData.VoMusic = config.voMusic;
                    }
                }
                catch
                {

                }
            }
            else
            {
                var content = JsonConvert.SerializeObject(new AutorunConfig());
                File.WriteAllText(configPath, content);
            }

            yield return null;
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            Request<VolumeContext> r = new Request<VolumeContext>();
            
            yield return context.UserRequest.Request(r, "Volume Master");

            if(r.Succeed)
            {
                var o = r.Object;
                VolumeData.VoClick = o.voClick;
                VolumeData.VoFlick = o.voFlick;
                VolumeData.VoRail = o.voRail;
                VolumeData.VoMusic = o.voMusic;

                var config = new AutorunConfig()
                {
                    voClick = o.voClick,
                    voFlick = o.voFlick,
                    voMusic = o.voMusic,
                    voRail = o.voRail
                };
                var content = JsonConvert.SerializeObject(config);
                File.WriteAllText(PathUtil.GetConfigPath("VolumeMaster"), content);
            }
        }
    }
}
