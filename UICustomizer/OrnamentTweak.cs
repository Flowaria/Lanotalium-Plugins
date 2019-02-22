using Lanotalium.Plugin;
using Lanotalium.Plugin.Simple.Wrapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Flowaria.Lanotalium.Plugin.Tweak
{
    public class OrnamentTweak : ILanotaliumPlugin
    {
        public string Name(Language language)
        {
            return "UiTweak - Tuner Ornaments";
        }

        public string Description(Language language)
        {
            return "Add Ornaments to tuner";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            if (context.IsProjectLoaded)
            {
                var judgeline = GameObject.Find("Tuner/JudgeLine");
                if (judgeline != null && GameObject.Find("JudgelineGlow") == null)
                {
                    var r = new Resources.OrnamentResources();
                    yield return ResourceBundle.LoadFromBundle<Resources.OrnamentResources>(Application.streamingAssetsPath + "/Assets/uitweak/uitweak.screen", x => r = x);

                    GameObject.Instantiate(r.Prefab_JudgeGlow, judgeline.transform).AddComponent<GlowManager>().c = context;
                    var glow = GameObject.Instantiate(r.Prefab_JudgeOrnament, judgeline.transform);
                    glow.name = "JudgelineGlow";
                }
            }

            yield return null;
        }
    }

    public class GlowManager : MonoBehaviour
    {
        public LanotaliumContext c;
        void Update()
        {
            gameObject.GetComponent<Animator>().speed = c.TunerManager.BpmManager.CurrentBpm / 60;
        }
    }
}
