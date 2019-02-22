using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyRequest;
using Lanotalium.Chart;
using Lanotalium.Plugin;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Flowaria.Lanotalium.Plugin
{
    public class VideoBGA : ILanotaliumPlugin
    {
        public string Name(Language language)
        {
            return "VideoBGA";
        }

        public string Description(Language language)
        {
            return "VideoBGA";
        } 

        public IEnumerator Process(LanotaliumContext context)
        {
            var result = new ChartLoadResult();
            result.isBackgroundGrayLoaded = true;
            result.isBackgroundLinearLoaded = false;
            result.isBackgroundLoaded = false;
            result.isBackgroundVideoDetected = true;
            result.isChartLoaded = true;
            result.isMusicLoaded = true;
            var videodata = new ChartBackground();
            videodata.VideoPath = Application.dataPath + "/StreamingAssets/stasis.mp4";
            context.TunerManager.BackgroundManager.BackgroundUpdator();
            context.TunerManager.BackgroundManager.Initialize(videodata, result);

            context.EditorManager.MusicPlayerWindow.InitializeVideo();
            context.EditorManager.MusicPlayerWindow.MusicPlayer.Stop();
            context.EditorManager.MusicPlayerWindow.Time = 0.0f;

            context.TunerManager.BackgroundManager.ColorImg.color = new Color(1, 1, 1, 1);
            context.TunerManager.BackgroundManager.GrayImg.color = new Color(1, 1, 1, 0);
            context.TunerManager.BackgroundManager.LinearImg.color = new Color(1, 1, 1, 0);

            var video = GameObject.Find("BackgroundVideo");
            var bga = GameObject.Find("BackgroundManager/Background");

            video.GetComponent<VideoPlayer>().renderMode = VideoRenderMode.MaterialOverride;
            video.GetComponent<VideoPlayer>().targetMaterialRenderer = bga.AddComponent<SpriteRenderer>();

            while (context.IsProjectLoaded)
            {
                yield return null;
            }

            yield return null;
        }
    }
}
