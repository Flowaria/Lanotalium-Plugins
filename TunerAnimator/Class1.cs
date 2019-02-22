using Lanotalium.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunerAnimator
{
    public class TunerAnimator : ILanotaliumPlugin
    {
        public string Description(Language language)
        {
            return "description";
        }

        public string Name(Language language)
        {
            return "Tuner Animation";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            yield return null;
        }
    }
}
