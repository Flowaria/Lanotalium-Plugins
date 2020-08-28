using EasyRequest;
using Lanotalium.Chart;
using Lanotalium.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Flowaria.Lanotalium.Plugin.Tweak
{
    public class SpecialPaste
    {
        [Name("How many times should be paste")]
        [Range(1, 2000)]
        public int Count = 1;

        [Name("└ Timing offset (zero will paste all in same timing)")]
        [Range(-500.0f, 500.0f)]
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

    public class FormulaPaste
    {
        [Name("How many times should be paste")]
        [Range(1, 2000)]
        public int Count = 1;

        [Name("Tap Note Degree")]
        public string FTapDegree = "";

        [Name("Tap Note Timing")]
        public string FTapTiming = "";
    }

    public class CopyPaster
    {
        LanotaliumContext context;
        EditorTweakCfg cfg;
        public CopyPaster(LanotaliumContext context, EditorTweakCfg cfg)
        {
            this.context = context;
        }

        List<LanotaTapNote> taps = new List<LanotaTapNote>();
        List<LanotaHoldNote> holds = new List<LanotaHoldNote>();
        List<LanotaCameraBase> motions = new List<LanotaCameraBase>();
        float timing_fastest = 999999999.0f;

        public IEnumerator CopyAll()
        {
            taps.Clear();
            holds.Clear();
            motions.Clear();
            timing_fastest = 999999999.0f;

            int i = 0, ii = 0, iii = 0;
            foreach (var note in context.OperationManager.SelectedTapNote.ToArray())
            {
                i++;
                taps.Add(note.DeepCopy());
                if (note.Time < timing_fastest)
                    timing_fastest = note.Time;
                context.OperationManager.DeSelectTapNote(note);
            }
            foreach (var note in context.OperationManager.SelectedHoldNote.ToArray())
            {
                ii++;
                holds.Add(note.DeepCopy());
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

            if (cfg.CV_DebugCounter)
            {
                context.MessageBox.ShowMessage(String.Format("[Ctrl+CV] Copied!\nTap Notes: {0}\nHold Notes: {1}\nMotions: {2}", i, ii, iii));
            }
            yield return null;
        }

        public IEnumerator PasteAll(float timing, float deg_offset = 0.0f, float radius_multiply = 1.0f)
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
                    context.OperationManager.AddRotation(nmotion, false, false, true);
                }

                else if (motion is LanotaCameraXZ)
                {
                    var nmotion = (motion as LanotaCameraXZ).DeepCopy();
                    nmotion.Time = nmotion.Time - timing_fastest + timing;
                    context.OperationManager.AddHorizontal(nmotion, false, false, true);
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

        //Show Special Paste Window
        public IEnumerator ShowSpecialPaste()
        {
            Request<SpecialPaste> request = new Request<SpecialPaste>();
            yield return context.UserRequest.Request(request, "Special Paste");

            if (request.Succeed)
            {
                var result = request.Object;
                float time = context.EditorManager.MusicPlayerWindow.CurrentTime;
                for (int i = 1; i <= result.Count; i++)
                    yield return PasteAll(time + (result.TimeOffset * (i - 1)), result.DegEach ? result.DegOffset * i : result.DegOffset, (float)Math.Pow(result.RadiusMultiply, i));
            }
        }
    }
}
