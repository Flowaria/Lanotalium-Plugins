using EasyRequest;
using Lanotalium.Plugin;
using Lanotalium.Plugin.Simple.HotKey;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Flowaria.Lanotalium.Plugin.Tweak
{
    public class SoundTypeData
    {
        [Name("Metronome Sound - 0: click, 1: flick in, 2: flick out")]
        [Range(0, 2)]
        public int Type;
    }

    public class BeatlineMetronome : HotKeyManager
    {
        public BeatlineMetronome()
        {
            RegisterHotKey();
        }
    }

    public class BeatlineButtonManager : MonoBehaviour, IPointerClickHandler
    {
        public LanotaliumContext context;

        public void OnPointerClick(PointerEventData e)
        {
            if (e.button == PointerEventData.InputButton.Right)
            {
                StartCoroutine("AskForMetronomeSound");
            }
            
        }

        public IEnumerable AskForMetronomeSound()
        {
            Request<SoundTypeData> request = new Request<SoundTypeData>();
            yield return context.UserRequest.Request(request, "Choose Your sound type");

            int type = 0;
            if (request.Succeed)
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
                    float delay = (comp.FindPrevOrNextBeatline(time, true) - time) * (1 / context.EditorManager.MusicPlayerWindow.Pitch);

                    if (delay > 0)
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
}
