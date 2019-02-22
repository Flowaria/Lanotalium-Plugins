using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Lanotalium.Plugin.Simple.PluginProcess
{
    public class ObjectProcess : MonoBehaviour
    {
        LanotaliumContext context;
        public ObjectProcess(LanotaliumContext context)
        {
            this.context = context;
        }
    }
}
