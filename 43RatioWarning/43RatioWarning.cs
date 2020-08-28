using Lanotalium.Plugin;
using Lanotalium.Plugin.Simple.Wrapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace _43RatioWarning
{
    public class Resources
    {
        [ResourceName("43Canvas.prefab")]
        public GameObject Prefab_43Canvas;
    }

    public class _43RatioWarning : ILanotaliumPlugin
    {
        public string Name(Language language)
        {
            return "4:3 Warning Border Toggle";
        }

        public string Description(Language language)
        {
            return "4.3 In the Middle of the LAW";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            if (context.IsProjectLoaded)
            {
                GameObject obj = null;
                if ((obj = GameObject.Find("43Canvas")) != null)
                {
                    GameObject.Destroy(obj);
                }
                else
                {
                    obj = GameObject.Find("LimTunerManager");
                    Resources r = null;
                    yield return ResourceBundle.LoadFromBundle<Resources>(Application.streamingAssetsPath + "/Assets/4.3warning", x => r = x);
                    obj = GameObject.Instantiate(r.Prefab_43Canvas, obj.transform);
                    obj.name = "43Canvas";

                    obj.GetComponent<Canvas>().worldCamera = GameObject.Find("LimTunerCamera").GetComponent<Camera>();
                }
            }
        }
    }
}
