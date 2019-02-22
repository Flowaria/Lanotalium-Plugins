using EasyRequest;
using Lanotalium.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
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
        public float voClick = Class1.VoClick;

        [Name("Flick note Volume (default: 0.2)")]
        [Range(0.0f, 1.0f)]
        public float voFlick = Class1.VoFlick;

        [Name("Rail note Volume (default: 0.2)")]
        [Range(0.0f, 1.0f)]
        public float voRail = Class1.VoRail;

        [Name("Music Volume (default: 0.4)")]
        [Range(0.0f, 1.0f)]
        public float voMusic = Class1.VoMusic;
    }

    public class Class1 : ILanotaliumPlugin
    {
        public static float VoClick
        {
            get
            {
                var obj = GameObject.Find("AudioEffectManager/Click");
                if(obj != null)
                {
                    return obj.GetComponent<AudioSource>().volume;
                }
                return 0.2f;
            }
            set
            {
                var obj = GameObject.Find("AudioEffectManager/Click");
                if (obj != null)
                {
                    obj.GetComponent<AudioSource>().volume = value;
                }
            }
        }

        public static float VoFlick
        {
            get
            {
                var obj = GameObject.Find("AudioEffectManager/Flickin");
                if (obj != null)
                {
                    return obj.GetComponent<AudioSource>().volume;
                }
                return 0.2f;
            }
            set
            {
                var obj = GameObject.Find("AudioEffectManager/Flickin");
                var obj2 = GameObject.Find("AudioEffectManager/Flickout");
                if (obj != null && obj2 != null)
                {
                    obj.GetComponent<AudioSource>().volume = value ;
                    obj2.GetComponent<AudioSource>().volume = value;
                }
            }
        }

        public static float VoRail
        {
            get
            {
                var obj = GameObject.Find("AudioEffectManager/Rail");
                if (obj != null)
                {
                    return obj.GetComponent<AudioSource>().volume;
                }
                return 0.2f;
            }
            set
            {
                var obj = GameObject.Find("AudioEffectManager/Rail");
                var obj2 = GameObject.Find("AudioEffectManager/Railend");
                if (obj != null && obj2 != null)
                {
                    obj.GetComponent<AudioSource>().volume = value;
                    obj2.GetComponent<AudioSource>().volume = value;
                }
            }
        }

        public static float VoMusic
        {
            get
            {
                var obj = GameObject.Find("ChartMusic");
                if (obj != null)
                {
                    return obj.GetComponent<AudioSource>().volume;
                }
                return 0.4f;
            }
            set
            {
                var obj = GameObject.Find("ChartMusic");
                if (obj != null)
                {
                    obj.GetComponent<AudioSource>().volume = value;
                }
            }
        }

        public string Description(Language language)
        {
            return "Change volume";
        }

        public string Name(Language language)
        {
            return "Volume";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            System.Windows.Application.Run()
            Request<VolumeContext> r = new Request<VolumeContext>();
            
            yield return context.UserRequest.Request(r, "Volume Master");

            if(r.Succeed)
            {
                var o = r.Object;
                VoClick = o.voClick;
                VoFlick = o.voFlick;
                VoRail = o.voRail;
                VoMusic = o.voMusic;
            }
        }
    }
}
