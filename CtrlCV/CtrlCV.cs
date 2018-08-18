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
    public class Preferences
    {
        [Name("Use Sound Effect for key pressed event")]
        public bool UseSoundEffect = true;

        [Name("Notify the count of copied elements")]
        public bool CopyPasteNotification = false;
    }

    public class SpecialPaste
    {
        [Name("How many times should be paste")]
        [Range(1, 2000)]
        public int Count = 1;

        [Name("└ Timing offset (zero will paste all in same timing)")]
        [Range(-500.0f,500.0f)]
        public float TimeOffset = 1.0f;

        [Name("(Tap, Hold Note) Degree offset")]
        [Range(-360.0f, 360.0f)]
        public float DegOffset = 0.0f;

        [Name("└ Is additive for each paste?")]
        public bool DegEach = false;

        [Name("(Horizontal) Multiply Radius for each paste")]
        [Range(0, 1000)]
        public float RadiusMultiply = 1.0f;
    }

    public class CtrlCV : ILanotaliumPlugin
    {
        public static bool UseSoundEffect = true;
        public static bool CopyPasteNotification = false;

        public string Name(Language language)
        {
            switch (language)
            {
                case Language.English:
                    return "Ctrl+CV";
                case Language.简体中文:
                    return "Ctrl+CV";
                default:
                    return "Ctrl+CV";
            }
        }

        public string Description(Language language)
        {
            switch (language)
            {
                case Language.English:
                    return "ctrl+cv is quite quick feature you know?";
                case Language.简体中文:
                    return "ctrl+cv is quite quick feature you know?";
                default:
                    return "ctrl+cv is quite quick feature you know?";
            }
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            bool enable = true;
            context.MessageBox.ShowMessage("[Ctrl+CV] Plugin Started!\n- Enable, Disable entire Hotkey, press F3\n- Copy all the motions and notes, press ctrl+c\n- Paste what you selected, press ctrl+v\n- Use advanced paste, press alt+v\n- Change setting of the plugin, press alt+s\n- Stop the Plugin, press alt+x");
            while(!Input.GetKey(KeyCode.LeftAlt) || !Input.GetKey(KeyCode.X))
            {
                if(Input.GetKey(KeyCode.F3))
                {
                    if (UseSoundEffect)
                        context.TunerManager.AudioEffectManager.PlayFlickOut();

                    enable = !enable;
                    if(enable)
                    {
                        context.MessageBox.ShowMessage("[Ctrl+CV] Hotkey Enabled!");
                    }
                    else
                    {
                        context.MessageBox.ShowMessage("[Ctrl+CV] Hotkey Disabled!");
                    }
                    yield return new WaitForSecondsRealtime(0.5f);
                }
                if(enable)
                {
                    //Copy
                    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.C))
                    {
                        if (UseSoundEffect)
                            context.TunerManager.AudioEffectManager.PlayFlickOut();
                        yield return CopyAll(context);
                        yield return new WaitForSecondsRealtime(0.75f);
                    }

                    //Paste
                    else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.V))
                    {
                        if (UseSoundEffect)
                            context.TunerManager.AudioEffectManager.PlayFlickOut();
                        yield return PasteAll(context, context.EditorManager.MusicPlayerWindow.CurrentTime);
                        yield return new WaitForSecondsRealtime(0.75f);
                    }

                    //Special Paste
                    if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.V))
                    {
                        if (UseSoundEffect)
                            context.TunerManager.AudioEffectManager.PlayFlickOut();
                        yield return ShowSpecialPaste(context);
                        yield return new WaitForSecondsRealtime(0.75f);
                    }

                    //Preferences
                    else if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.S))
                    {
                        yield return ShowPreferences(context);
                    }
                }     
                yield return null;
            }
            context.MessageBox.ShowMessage("[Ctrl+CV] Plugin Stopped!");
        }

        List<LanotaTapNote> taps = new List<LanotaTapNote>();
        List<LanotaHoldNote> holds = new List<LanotaHoldNote>();
        List<LanotaCameraBase> motions = new List<LanotaCameraBase>();
        float timing_fastest = 999999999.0f;
        public IEnumerator CopyAll(LanotaliumContext context)
        {
            taps.Clear();
            holds.Clear();
            motions.Clear();
            timing_fastest = 999999999.0f;

            int i = 0, ii = 0, iii = 0;
            foreach(var note in context.OperationManager.SelectedTapNote.ToArray())
            {
                i++;
                taps.Add(note);
                if (note.Time < timing_fastest)
                    timing_fastest = note.Time;
                context.OperationManager.DeSelectTapNote(note);
            }
            foreach(var note in context.OperationManager.SelectedHoldNote.ToArray())
            {
                ii++;
                holds.Add(note);
                if (note.Time < timing_fastest)
                    timing_fastest = note.Time;
                context.OperationManager.DeSelectHoldNote(note);
            }
            foreach (var motion in context.OperationManager.SelectedMotions.ToArray())
            {
                iii++;
                motions.Add(motion);
                if (motion.Time < timing_fastest)
                    timing_fastest = motion.Time;
                context.OperationManager.DeSelectMotion(motion);
            }

            if(CopyPasteNotification)
            {
                context.MessageBox.ShowMessage(String.Format("[Ctrl+CV] Copied!\nTap Notes: {0}\nHold Notes: {1}\nMotions: {2}", i, ii, iii));
            }
            yield return null;
        }

        public IEnumerator PasteAll(LanotaliumContext context, float timing, float deg_offset = 0.0f, float radius_multiply = 1.0f)
        {
            foreach (var note in taps)
            {
                var nnote = note.DeepCopy();
                nnote.Time = nnote.Time - timing_fastest + timing;
                nnote.Degree += deg_offset;
                context.OperationManager.AddTapNote(nnote, false, true, true);
            }
            foreach (var note in holds)
            {
                var nnote = note.DeepCopy();
                nnote.Time = nnote.Time - timing_fastest + timing;
                nnote.Degree += deg_offset;
                context.OperationManager.AddHoldNote(nnote, false, true, true);
            }
            foreach (var motion in motions)
            {
                if (motion is LanotaCameraRot)
                {
                    var nmotion = (motion as LanotaCameraRot).DeepCopy();
                    nmotion.Time = nmotion.Time - timing_fastest + timing;
                    context.OperationManager.AddRotation(nmotion, false, false ,true);
                }
                    
                else if (motion is LanotaCameraXZ)
                {
                    var nmotion = (motion as LanotaCameraXZ).DeepCopy();
                    nmotion.Time = nmotion.Time - timing_fastest + timing;
                    context.OperationManager.AddHorizontal(nmotion, false , false, true);
                    context.OperationManager.SetHorizontalRadius(nmotion, nmotion.ctp1 * radius_multiply);
                }
                else if (motion is LanotaCameraY)
                {
                    var nmotion = (motion as LanotaCameraY).DeepCopy();
                    nmotion.Time = nmotion.Time - timing_fastest + timing;
                    context.OperationManager.AddVertical(nmotion, false, false, true);
                }
            }
            yield return null;
        }

        //Show Preferences Window
        public IEnumerator ShowPreferences(LanotaliumContext context)
        {
            Request<Preferences> request = new Request<Preferences>();
            yield return context.UserRequest.Request(request, "Preferences");

            if (request.Succeed)
            {
                UseSoundEffect = request.Object.UseSoundEffect;
                CopyPasteNotification = request.Object.CopyPasteNotification;
            }
        }
        
        //Show Special Paste Window
        public IEnumerator ShowSpecialPaste(LanotaliumContext context)
        {
            Request<SpecialPaste> request = new Request<SpecialPaste>();
            yield return context.UserRequest.Request(request, "Special Paste");

            if (request.Succeed)
            {
                var result = request.Object;
                float time = context.EditorManager.MusicPlayerWindow.CurrentTime;
                for(int i = 1;i<=result.Count;i++)
                    yield return PasteAll(context, time+(result.TimeOffset*(i-1)), result.DegEach? result.DegOffset*i: result.DegOffset, (float)Math.Pow(result.RadiusMultiply, i));
            }
        }
    }
}
