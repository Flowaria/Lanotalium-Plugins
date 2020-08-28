using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VolumeMaster
{
    public static class VolumeData
    {
        public static float VoClick
        {
            get
            {
                var obj = GameObject.Find("AudioEffectManager/Click");
                if (obj != null)
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
                    obj.GetComponent<AudioSource>().volume = value;
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
    }
}
