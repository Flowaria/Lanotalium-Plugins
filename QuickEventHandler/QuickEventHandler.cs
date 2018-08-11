using Lanotalium.Plugin.Events.Chart;
using Lanotalium.Plugin.Events.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lanotalium.Plugin.Events
{
    public class QuickEventHandler : ILanotaliumPlugin
    {
        public string Description(Language language)
        {
            return "Extension for Support Event handle in Lanotalium Plugin";
        }

        public string Name(Language language)
        {
            return "Quick Event Handler API";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            GameObject manager;

            if ((manager = GameObject.Find("ChartEventHandler")) == null)
                GameObject.Destroy(manager);

            manager = new GameObject("ChartEventHandler");
            manager.AddComponent<ChartEventManager>();
            manager.GetComponent<ChartEventManager>().context = context;

            foreach (var file in Directory.GetFiles(Application.dataPath + "/StreamingAssets/Plugins"))
            {
                Assembly assembly = Assembly.LoadFrom(file);
                Type[] types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.GetInterface("IBpmEvent") != null)
                    {
                        IBpmEvent e = Activator.CreateInstance(type) as IBpmEvent;
                        manager.GetComponent<ChartEventManager>().EventBpm.Add(e);
                    }
                }
            }

            manager.SetActive(true);
            yield return null;
        }
    }
}
