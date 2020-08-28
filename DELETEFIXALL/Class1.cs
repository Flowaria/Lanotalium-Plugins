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
    public class Class1 : ILanotaliumPlugin
    {
        public string Name(Language language)
        {
            return "DISABLE THE DEMON (or put it back)";
        }

        public string Description(Language language)
        {
            return "THE 5TH DEMON, \"FIXALL\" IS HERE BROTHER";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            if(!context.IsProjectLoaded)
            {
                yield break;
            }

            var obj = GameObject.Find("FixAll");
            if(obj != null)
            {
                var button = obj.GetComponent<Button>();
                button.interactable = !button.interactable;
            }

            yield return null;
        }
    }
}
