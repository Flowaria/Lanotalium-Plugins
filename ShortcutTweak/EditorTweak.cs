using Lanotalium.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Flowaria.Lanotalium.Plugin
{
    public class EditorTweak : ILanotaliumPlugin
    {
        public string Name(Language language)
        {
            return "Editor Tweak - Startup (Turn off)";
        }

        public string Description(Language language)
        {
            return "Ready to boost your work :D";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            var manager = GameObject.Find("EditorTweak");
            if (manager == null)
            {
                manager = new GameObject("EditorTweak");
                manager.AddComponent<TweakManager>();
                manager.GetComponent<TweakManager>().context = context;
                manager.SetActive(true);

                context.MessageBox.ShowMessage("Turned on");
            }
            else
            {
                GameObject.Destroy(manager);

                context.MessageBox.ShowMessage("Turned off");
            }
            yield return true;
        }
    }
}
