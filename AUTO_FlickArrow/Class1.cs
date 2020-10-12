using Flowaria.AutorunPlugin;
using JetBrains.Annotations;
using Lanotalium.Plugin;
using Lanotalium.Plugin.Simple.Wrapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace AUTO_FlickArrow
{
    [PluginName("FlickArrow")]
    public class Class1 : ILanotaliumAutorun
    {
        public IEnumerator OnLoaded(string configPath, LanotaliumContext context)
        {
            if(!context.IsProjectLoaded)
            {
                yield break;
            }

            var assetpath = Application.streamingAssetsPath + "/Assets/flickarrow";
            FlickResources r = null;
            yield return ResourceBundle.LoadFromBundle<FlickResources>(assetpath, x => r = x);

            AddScroll(r.Prefab_oIn0, -1.5f);
            AddScroll(r.Prefab_oIn1, -1.5f);
            AddScroll(r.Prefab_oIn2, -1.5f);
            AddScroll(r.Prefab_oIn3, -1.5f);

            AddScroll(r.Prefab_oOut0, 1.5f);
            AddScroll(r.Prefab_oOut1, 1.5f);
            AddScroll(r.Prefab_oOut2, 1.5f);
            AddScroll(r.Prefab_oOut3, 1.5f);

            AddScroll(r.Prefab_cIn0, -1.5f);
            AddScroll(r.Prefab_cIn1, -1.5f);
            AddScroll(r.Prefab_cIn2, -1.5f);
            AddScroll(r.Prefab_cIn3, -1.5f);

            AddScroll(r.Prefab_cOut0, 1.5f);
            AddScroll(r.Prefab_cOut1, 1.5f);
            AddScroll(r.Prefab_cOut2, 1.5f);
            AddScroll(r.Prefab_cOut3, 1.5f);

            

            var tapManager = context.TunerManager.TapNoteManager;
            tapManager.oT2S0 = r.Prefab_oIn0;
            tapManager.oT2S1 = r.Prefab_oIn1;
            tapManager.oT2S2 = r.Prefab_oIn2;
            tapManager.oT2S3 = r.Prefab_oIn3;

            tapManager.oT3S0 = r.Prefab_oOut0;
            tapManager.oT3S1 = r.Prefab_oOut1;
            tapManager.oT3S2 = r.Prefab_oOut2;
            tapManager.oT3S3 = r.Prefab_oOut3;

            tapManager.cT2S0 = r.Prefab_cIn0;
            tapManager.cT2S1 = r.Prefab_cIn1;
            tapManager.cT2S2 = r.Prefab_cIn2;
            tapManager.cT2S3 = r.Prefab_cIn3;

            tapManager.cT3S0 = r.Prefab_cOut0;
            tapManager.cT3S1 = r.Prefab_cOut1;
            tapManager.cT3S2 = r.Prefab_cOut2;
            tapManager.cT3S3 = r.Prefab_cOut3;

            //regenerate old notes
            foreach(var note in tapManager.TapNote)
            {
                if(note.Type == 2 || note.Type == 3)
                {
                    GameObject.Destroy(note.TapNoteGameObject);
                    tapManager.InstantiateNote(note);
                }
                yield return null;
            }
        }

        public void AddScroll(GameObject obj, float speed)
        {
            var scroll = obj.transform.Find("Arrow").gameObject.AddComponent<FlickArrowScroll>();
            scroll.scrollSpeed = speed;

            scroll = obj.transform.Find("Arrow_back").gameObject.AddComponent<FlickArrowScroll>();
            scroll.scrollSpeed = speed;
        }
    }

    public class FlickResources
    {
        [ResourceName("oFlickOut0")]
        public GameObject Prefab_oOut0;

        [ResourceName("oFlickOut1")]
        public GameObject Prefab_oOut1;

        [ResourceName("oFlickOut2")]
        public GameObject Prefab_oOut2;

        [ResourceName("oFlickOut3")]
        public GameObject Prefab_oOut3;

        //

        [ResourceName("cFlickOut0")]
        public GameObject Prefab_cOut0;

        [ResourceName("cFlickOut1")]
        public GameObject Prefab_cOut1;

        [ResourceName("cFlickOut2")]
        public GameObject Prefab_cOut2;

        [ResourceName("cFlickOut3")]
        public GameObject Prefab_cOut3;

        //

        [ResourceName("oFlickIn0")]
        public GameObject Prefab_oIn0;

        [ResourceName("oFlickIn1")]
        public GameObject Prefab_oIn1;

        [ResourceName("oFlickIn2")]
        public GameObject Prefab_oIn2;

        [ResourceName("oFlickIn3")]
        public GameObject Prefab_oIn3;

        //

        [ResourceName("cFlickIn0")]
        public GameObject Prefab_cIn0;

        [ResourceName("cFlickIn1")]
        public GameObject Prefab_cIn1;

        [ResourceName("cFlickIn2")]
        public GameObject Prefab_cIn2;

        [ResourceName("cFlickIn3")]
        public GameObject Prefab_cIn3;
    }

    public class FlickArrowScroll : MonoBehaviour
    {
        public float scrollSpeed = 1.5F;
        private Renderer rend;
        private float randOffset = 0;
        void Start()
        {
            rend = GetComponent<Renderer>();
            randOffset = UnityEngine.Random.Range(-1.0f, 1.0f);
        }
        void Update()
        {
            float offset = Time.time * scrollSpeed * 1.3f + randOffset;
            rend.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
        }
    }
}
