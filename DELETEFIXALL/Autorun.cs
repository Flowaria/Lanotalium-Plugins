using Flowaria.AutorunPlugin;
using Lanotalium.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace DELETEFIXALL
{
    [PluginName("DeleteFixAll")]
    public class Autorun : ILanotaliumAutorun
    {
        public IEnumerator OnLoaded(string configPath, LanotaliumContext context)
        {
            if(!context.IsProjectLoaded)
            {
                yield break;
            }

            var objects = Resources.FindObjectsOfTypeAll<Button>();
            foreach (var obj in objects)
            {
                if(obj.gameObject.name.Equals("FixAll"))
                {
                    obj.interactable = false;
                }
            }

            yield return null;
        }
    }
}
