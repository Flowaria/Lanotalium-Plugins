using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyRequest;
using Lanotalium.Chart;
using Lanotalium.Plugin;
using MenuAPI;
using UnityEngine;

namespace Flowaria.Lanotalium.Plugin
{
    public class SoundTypeData
    {
        [Range(0,2)]
        public int Type;
    }

    public class BeatlineMetronome : ILanotaliumPlugin
    {
        public string Name(Language language)
        {
            switch (language)
            {
                case Language.English:
                    return "Beatline Metronome";
                case Language.简体中文:
                    return "Beatline Metronome";
                default:
                    return "Beatline Metronome";
            }
        }

        public string Description(Language language)
        {
            switch (language)
            {
                case Language.English:
                    return "You can use metronome by lanotalium bpm system";
                case Language.简体中文:
                    return "You can use metronome by lanotalium bpm system";
                default:
                    return "You can use metronome by lanotalium bpm system";
            }
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            if(!context.IsProjectLoaded)
            {
                context.MessageBox.ShowMessage("Project is not loaded! Try again when you loaded Project");
            }
            else
            {
                int type = 1;
                Request<SoundTypeData> request = new Request<SoundTypeData>();
                yield return context.UserRequest.Request(request, "Please choose sound type (0: click, 1: flick in, 2: flick out)");
                
                if(request.Succeed)
                    type = request.Object.Type;
                else
                    context.MessageBox.ShowMessage("Error Occured! Using Default setting! (click)");

                var comp = context.EditorManager.InspectorWindow.ComponentBpm;
                if (!comp.EnableBeatline)
                    comp.EnableBeatline = true;

                while (comp.EnableBeatline)
                {
                    if (!context.EditorManager.MusicPlayerWindow.IsPlaying)
                    {
                        yield return null;
                    }
                    else
                    {
                        float time = context.TunerManager.MediaPlayerManager.CurrentTime;
                        float delay = (comp.FindPrevOrNextBeatline(time, true) - time) * (1/context.EditorManager.MusicPlayerWindow.Pitch);

                        if(delay > 0)
                        {
                            yield return new WaitForSecondsRealtime(delay);

                            if (type == 0)
                                context.TunerManager.AudioEffectManager.PlayClick();
                            if (type == 1)
                                context.TunerManager.AudioEffectManager.PlayFlickIn();
                            if (type == 2)
                                context.TunerManager.AudioEffectManager.PlayFlickOut();
                        }

                        yield return null;
                    }
                }
            }
        }
        public class DeleteButton : ICreatorButton
        {
            public string DefaultName(Language language)
            {
                return "Enable Beatline Metronome";
            }

            public IEnumerator OnClick(LanotaliumContext context, GameObject Buttonobj)
            {
                yield return null;
            }
        }
    }
}
