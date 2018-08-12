using EasyRequest;
using Lanotalium.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Flowaria.Lanotalium.Plugin
{
    public class ParticleZCore : ILanotaliumPlugin
    {
        public string Name(Language lang)
        {
            return "ParticleZ - Switch";
        }

        public string Description(Language lang)
        {
            return "Turn on it god dammit!";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            var manager = Manager.Create(context);

            var Bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "Assets/lanotaparticle"));
            if (Bundle != null)
            {
                manager.GetComponent<ParticleManager>().PrefabHoldSparkle = Bundle.LoadAsset<GameObject>("Assets/Particle/HoldNoteSparkle.prefab");
            }
            yield return null;
        }
    }
    
    public class ParticleZPreferences : ILanotaliumPlugin
    {
        public string Name(Language lang)
        {
            return "ParticleZ - Preferences";
        }

        public string Description(Language lang)
        {
            return "Setting the ParticleZ";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            
            Request<ParticleManagerPreferences> r = new Request<ParticleManagerPreferences>();
            yield return context.UserRequest.Request(r, "Preferences Window");
            if (r.Succeed)
            {
                Manager.Setting(r.Object);
            }
            yield return null;
        }
    }

    public class Manager
    {
        public static GameObject Create(LanotaliumContext context)
        {
            if(!Exists())
            {
                var manager = new GameObject("pParticleManager");
                if(manager != null)
                {
                    manager.AddComponent<ParticleManager>();
                    manager.GetComponent<ParticleManager>().context = context;
                    manager.SetActive(true);
                    return manager;
                }
                return null;
            }
            else
            {
                return GameObject.Find("pParticleManager");
            }
        }

        public static void Setting(ParticleManagerPreferences pref)
        {
            if (Exists())
            {
                GameObject.Find("pParticleManager").GetComponent<ParticleManager>().pref = pref;
            }
        }

        public static bool Exists()
        {
            return GameObject.Find("pParticleManager") != null;
        }
    }
}
