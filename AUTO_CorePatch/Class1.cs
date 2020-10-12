using Flowaria.AutorunPlugin;
using Lanotalium.Plugin;
using Lanotalium.Plugin.Simple.Wrapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AUTO_CorePatch
{
    [PluginName("CorePatcher")]
    public class Class1 : ILanotaliumAutorun
    {
        public IEnumerator OnLoaded(string configPath, LanotaliumContext context)
        {
            var assetpath = Application.streamingAssetsPath + "/Assets/tunercore";
            CoreResource r = null;
            yield return ResourceBundle.LoadFromBundle<CoreResource>(assetpath, x => r = x);

            var coreObj = GameObject.Find("Tuner/Core");
            if(coreObj != null)
            {
                var spriteRend = coreObj.GetComponent<SpriteRenderer>();
                if(spriteRend != null)
                {
                    spriteRend.sprite = r.Prefab_CoreBigImage;
                    spriteRend.color = new Color(0.7f, 0.7f, 0.7f, 1.0f);
                }

                coreObj.transform.localScale = new Vector3(0.14f, 0.14f, 1.0f);
            }
        }
    }

    public class CoreResource
    {
        [ResourceName("core_big")]
        public Sprite Prefab_CoreBigImage;
    }
}
