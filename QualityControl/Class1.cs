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
    public class QualityControl : ILanotaliumPlugin
    {
        //
        public string Description(Language language)
        {
            throw new NotImplementedException();
        }

        public string Name(Language language)
        {
            return "UiTweak";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            QualitySettings.shadows = ShadowQuality.Disable;
            QualitySettings.SetQualityLevel(0, true);
            yield return null;        }
    }
}