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

namespace Flowaria.Lanotalium.Plugin
{
    public class RecordingSetting
    {
        [Name("Framerate (FPS)")]
        [Range(1, 200)]
        [Default(60)]
        public int FPS;

        [Name("Judge Color")]
        [Default(true)]
        public bool JudgeColor;

        [Name("Judge Sound")]
        [Default(true)]
        public bool JudgeSound;

        [Name("Hide Cursor")]
        [Default(true)]
        public bool HideCursor;
    }
    public class FrameRecorder : ILanotaliumPlugin
    {
        public string Name(Language language)
        {
            return "Frame Based Recorder";
        }

        public string Description(Language language)
        {
            return "recording screen in high frame even if your pc is toast";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            Request<RecordingSetting> request = new Request<RecordingSetting>();
            yield return context.UserRequest.Request(request, "Record Setting Window");

            if (request.Succeed)
            {
                var result = request.Object;
                //Cursor.visible = !result.HideCursor;
                context.EditorManager.TunerWindow.DisplayManager.FullScreenTuner = true;
                context.EditorManager.TunerWindow.TunerHeadManager.HeadRect.SetSizeWithCurrentAnchors(0, 0.0f);

                while (!Input.GetKey(KeyCode.F10) && !Input.GetKey(KeyCode.Escape))
                {

                    context.EditorManager.MusicPlayerWindow.Time = 0.0f;
                    yield return null;
                }
            }
        }
    }
}
