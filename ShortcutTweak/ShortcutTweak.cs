using Lanotalium.Plugin;
using ShortcutTweak;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Flowaria.Lanotalium.Plugin
{
    public class ShortcutTweak : ILanotaliumPlugin
    {
        public string Name(Language language)
        {
            return "Shortcut Tweak - Startup (Turn off)";
        }

        public string Description(Language language)
        {
            return "Ready to boost your work";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            var manager = GameObject.Find("ShortcutTweak");
            if (manager == null)
            {
                manager = new GameObject("ShortcutTweak");
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

    public class ShortcutTweakSetting : ILanotaliumPlugin
    {
        public string Name(Language language)
        {
            return "Shortcut Tweak - Setting";
        }

        public string Description(Language language)
        {
            return "Remap the key, Toggle the Feature";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            throw new NotImplementedException();
        }
    }
}
