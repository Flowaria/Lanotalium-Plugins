using Lanotalium.Chart;
using Lanotalium.Plugin.Events.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lanotalium.Plugin.Events.Manager
{
    public class ChartEventManager : MonoBehaviour
    {
        public LanotaliumContext context;

        public List<INoteEvent> EventNote = new List<INoteEvent>();
        public List<IHorizontalMotionEvent> EventHorizontal = new List<IHorizontalMotionEvent>();
        public List<IVerticalMotionEvent> EventVertical = new List<IVerticalMotionEvent>();
        public List<IRotationMotionEvent> EventRotation = new List<IRotationMotionEvent>();

        public List<IScrollSpeedEvent> EventScrollSpeed = new List<IScrollSpeedEvent>();
        public List<IBpmEvent> EventBpm = new List<IBpmEvent>();
        public List<IBeatlineEvent> EventBeatline = new List<IBeatlineEvent>();
        
        void Start()
        {
            CurrentBpm = context.TunerManager.BpmManager.CurrentBpm;
            context.MessageBox.ShowMessage(EventBpm.Count.ToString());
        }

        void Update()
        {
            UpdateTapNote();
            UpdateHoldNote();

            UpdateMotion();

            UpdateScrollSpeed();
            UpdateBpm();
        }

        Dictionary<LanotaTapNote, bool> ClickedTapNote = new Dictionary<LanotaTapNote, bool>();
        void UpdateTapNote()
        {
            foreach(var note in context.TunerManager.TapNoteManager.TapNote)
            {

            }
        }

        void UpdateHoldNote()
        {
            foreach (var note in context.TunerManager.HoldNoteManager.HoldNote)
            {

            }
        }

        void UpdateMotion()
        {
            foreach (var motion in context.TunerManager.CameraManager.Horizontal)
            {

            }

            foreach (var motion in context.TunerManager.CameraManager.Vertical)
            {

            }

            foreach (var motion in context.TunerManager.CameraManager.Rotation)
            {

            }
        }

        void UpdateScrollSpeed()
        {
            foreach (var scroll in context.TunerManager.ScrollManager.Scroll)
            {

            }
        }

        float CurrentBpm;
        void UpdateBpm()
        {
            if (CurrentBpm != context.TunerManager.BpmManager.CurrentBpm)
            {
                LanotaChangeBpm bpm = null;

                for (int i = 0; i < context.TunerManager.BpmManager.Bpm.Count - 1; ++i)
                {
                    if (context.TunerManager.ChartTime > context.TunerManager.BpmManager.Bpm[i].Time && context.TunerManager.ChartTime <= context.TunerManager.BpmManager.Bpm[i + 1].Time)
                    {
                        bpm = context.TunerManager.BpmManager.Bpm[i];
                        break;
                    }
                    else
                    {
                        bpm = context.TunerManager.BpmManager.Bpm[context.TunerManager.BpmManager.Bpm.Count - 1];
                    }
                }
                if (bpm != null)
                {
                    CurrentBpm = bpm.Bpm;
                    foreach (var e in EventBpm)
                    {
                        StartCoroutine(e.OnBpmChanged(context, bpm));
                    }
                }
                
            }
        }
    }
}
