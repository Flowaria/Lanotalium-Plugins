using Lanotalium.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SEChange
{
    public class Class1 : ILanotaliumPlugin
    {
        public static AudioClip ClipClick
        {
            get
            {
                var obj = GameObject.Find("AudioEffectManager/Click");
                return obj?.GetComponent<AudioSource>().clip;
            }
            set
            {
                var obj = GameObject.Find("AudioEffectManager/Click");
                if (obj != null)
                {
                    obj.GetComponent<AudioSource>().clip = value;
                }
            }
        }

        public static AudioClip ClipFlickIn
        {
            get
            {
                var obj = GameObject.Find("AudioEffectManager/Flickin");
                return obj?.GetComponent<AudioSource>().clip;
            }
            set
            {
                var obj = GameObject.Find("AudioEffectManager/Flickin");
                if (obj != null)
                {
                    obj.GetComponent<AudioSource>().clip = value;
                }
            }
        }

        public static AudioClip ClipFlickOut
        {
            get
            {
                var obj = GameObject.Find("AudioEffectManager/Flickout");
                return obj?.GetComponent<AudioSource>().clip;
            }
            set
            {
                var obj2 = GameObject.Find("AudioEffectManager/Flickout");
                if (obj2 != null)
                {
                    obj2.GetComponent<AudioSource>().clip = value;
                }
            }
        }

        public static AudioClip VoRail
        {
            get
            {
                var obj = GameObject.Find("AudioEffectManager/Rail");
                return obj?.GetComponent<AudioSource>().clip;
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

        public void ddd()
        {

            var clip = AudioClip.Create("flickincustom", 0, 2, 192, false);
            clip.
        }
    }
}
