using EasyRequest;
using Lanotalium.Plugin;
using Lanotalium.Plugin.Simple;
using Lanotalium.Plugin.Simple.Wrapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GuideLines
{
    public class Class1 : ILanotaliumPlugin
    {
        public string Description(Language language)
        {
            return "Screen Guideline";
        }

        public string Name(Language language)
        {
            return "Screen Guideline";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            if(!context.IsProjectLoaded)
            {
                yield break;
            }

            Request<AskForGuide> request = new Request<AskForGuide>();
            yield return context.UserRequest.Request(request, "Line Setting");
            if (!request.Succeed)
            {
                yield break;
            }

            var result = request.Object;

            GuidelineResources resource = null;
            var resourcePath = Application.streamingAssetsPath + "/Assets/screenlineguides";
            yield return ResourceBundle.LoadFromBundle<GuidelineResources>(resourcePath, x => resource = x);

            var tunerManager = context.TunerManager.gameObject;
            if (tunerManager == null)
            {
                yield break;
            }

            var mgr = ExternalBehaviour<GuidelineManager>.Get(
                "ScreenGuideManager",
                resource.Prefab_ScreenSpace,
                tunerManager,
                out bool hasCreated
                );

            mgr.TunerObject = GameObject.Find("Tuner");
            mgr.TunerDot_Prefab = resource.Prefab_Dot;

            mgr.GetComponent<Canvas>().worldCamera = GameObject.Find("LimTunerCamera").GetComponent<Camera>();

            if (hasCreated)
            {
                mgr.Init();
            }

            if (!result.Hide)
            {
                mgr.Show();
                mgr.SetCounts(result.VerticalCount, result.HorizontalCount);
                mgr.SetColor(ColorUtil.FromInt(result.ColorR, result.ColorG, result.ColorB, result.ColorA));
            }
            else
            {
                mgr.Hide();
            }

            yield return true;
        }
    }

    public class GuidelineResources
    {
        [ResourceName("CenterDot")]
        public GameObject Prefab_Dot;

        [ResourceName("ScreenCanvas")]
        public GameObject Prefab_ScreenSpace;
    }

    public class AskForGuide
    {
        [Name("Hide")]
        public bool Hide = false;

        [Name("Vertical Line Number (0 will hide all lines)")]
        [Range(0, 64)]
        public int VerticalCount = 1;

        [Name("Horizontal Line Number (0 will hide all lines)")]
        [Range(0, 64)]
        public int HorizontalCount = 1;

        [Name("Color Red Param")]
        [Range(0, 255)]
        public int ColorR = 255;

        [Name("Color Green Param")]
        [Range(0, 255)]
        public int ColorG = 255;

        [Name("Color Blue Param")]
        [Range(0, 255)]
        public int ColorB = 255;

        [Name("Alpha Param")]
        [Range(0, 255)]
        public int ColorA = 255;

        //[Name("Allow Hotkey")]
        //public bool AllowHotkey = true;
    }
}
