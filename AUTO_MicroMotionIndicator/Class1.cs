using Flowaria.AutorunPlugin;
using Lanotalium.Plugin;
using Lanotalium.Plugin.Simple.Wrapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace AUTO_MicroMotionIndicator
{
    [PluginName("MicroMotionIndicator")]
    public class Class1 : ILanotaliumAutorun
    {
        public IEnumerator OnLoaded(string configPath, LanotaliumContext context)
        {
            if (!context.IsProjectLoaded)
            {
                yield break;
            }

            var assetpath = Application.streamingAssetsPath + "/Assets/timeline.mmotion.indicator";
            MarkerResources r = null;
            yield return ResourceBundle.LoadFromBundle<MarkerResources>(assetpath, x => r = x);

            var timelineManager = context.EditorManager.TimeLineWindow;

            r.Prefab_Marker.transform.SetParent(timelineManager.TimeLineObject.transform, false);

            var helper = timelineManager.TimeLineObject.AddComponent<TimelineObjectHelper>();
            helper.MarkerButton = r.Prefab_Marker.GetComponent<Button>();
            helper.Marker = r.Prefab_Marker;

            timelineManager.InstantiateAllTimeLine();
        }
    }

    public class MarkerResources
    {
        [ResourceName("TimelineMarker")]
        public GameObject Prefab_Marker;
    }
}
