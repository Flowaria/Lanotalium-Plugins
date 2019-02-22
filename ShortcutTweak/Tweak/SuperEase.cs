using EasyRequest;
using Lanotalium.Chart;
using Lanotalium.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Flowaria.Lanotalium.Plugin.Tweak
{
    public class BpmEase
    {
        [Name("From")]
        [Range(0.0001f, 1000.0f)]
        public float From = 1.0f;

        [Name("To")]
        [Range(0.0001f, 1000.0f)]
        public float To = 1.0f;

        [Name("Duration")]
        [Range(0.0001f, 600.0f)]
        public float Duration = 1.0f;

        [Name("Ease")]
        [Range(0, 12)]
        public int Ease = 0;

        [Name("Sample Count")]
        [Range(2, 64)]
        public int Sample = 16;

        [Name("Cencel the operation when meet Other BPM")]
        public bool SafetyLock = true;
    }

    public class SpeedEase
    {
        [Name("From")]
        [Range(-99999.0f, 99999.0f)]
        public float From = 1.0f;

        [Name("To")]
        [Range(-99999.0f, 99999.0f)]
        public float To = 1.0f;

        [Name("Duration")]
        [Range(0.0001f, 600.0f)]
        public float Duration = 1.0f;

        [Name("Ease")]
        [Range(0, 12)]
        public int Ease = 0;

        [Name("Sample Count")]
        [Range(2, 64)]
        public int Sample = 16;

        [Name("Cencel the operation when meet Other Scroll Speed")]
        public bool SafetyLock = true;
    }

    public class SuperEase
    {
        LanotaliumContext context;
        public SuperEase(LanotaliumContext context, EditorTweakCfg cfg)
        {
            this.context = context;
        }

        public bool ApplyEase(SpeedEase ease)
        {
            if (ease.SafetyLock)
            {
                foreach (var bpm in context.TunerManager.BpmManager.Bpm)
                {
                    if (context.TunerManager.ChartTime <= bpm.Time && bpm.Time <= context.TunerManager.ChartTime + ease.Duration)
                        return false;
                }
            }

            float TimeDelta = context.TunerManager.ChartTime - ease.Duration, SpeedDelta = ease.To - ease.From;

            AddScrollSpeed(context.TunerManager.ChartTime, ease.From);

            for (int i = 1; i < ease.Sample; i++)
            {
                float spercent = i / (ease.Sample - 1);
                AddBpm(context.TunerManager.ChartTime + TimeDelta * spercent, SpeedDelta * CalculateEasedCurve(spercent, ease.Ease));
            }

            return true;
        }

        public bool ApplyEase(BpmEase ease)
        {
            if(ease.SafetyLock)
            {
                foreach (var bpm in context.TunerManager.BpmManager.Bpm)
                {
                    if (context.TunerManager.ChartTime <= bpm.Time && bpm.Time <= context.TunerManager.ChartTime + ease.Duration)
                        return false;
                }
            }

            float TimeDelta = context.TunerManager.ChartTime - ease.Duration, BpmDelta = ease.To - ease.From;

            AddBpm(context.TunerManager.ChartTime, ease.From);

            for (int i = 1;i<ease.Sample;i++)
            {
                float spercent = i / (ease.Sample - 1);
                AddBpm(context.TunerManager.ChartTime + TimeDelta * spercent, BpmDelta * CalculateEasedCurve(spercent, ease.Ease));
            }
            return true;
        }

        private void AddBpm(float time, float bpm)
        {
            LanotaChangeBpm NewBpm = new LanotaChangeBpm
            {
                Time = time,
                Bpm = bpm
            };
            context.OperationManager.AddBpm(NewBpm, false);
        }

        private void AddScrollSpeed(float time, float speed)
        {
            LanotaScroll NewSpeed = new LanotaScroll
            {
                Time = time,
                Speed = speed
            };
            context.OperationManager.AddScrollSpeed(NewSpeed, false);
        }

        //From Lanotalium Source
        private float CalculateEasedCurve(float Percent, int Mode)
        {
            if (Percent >= 1.0) return 1.0f;
            else if (Percent <= 0.0) return 0.0f;
            switch (Mode)
            {
                case 0:
                    return Percent;
                case 1:
                    return Percent * Percent * Percent * Percent;
                case 2:
                    return -(Percent - 1) * (Percent - 1) * (Percent - 1) * (Percent - 1) + 1;
                case 3:
                    return (Percent < 0.5) ? (Percent * Percent * Percent * Percent * 8) : ((Percent - 1) * (Percent - 1) * (Percent - 1) * (Percent - 1) * -8 + 1);
                case 4:
                    return Percent * Percent * Percent;
                case 5:
                    return (Percent - 1) * (Percent - 1) * (Percent - 1) + 1;
                case 6:
                    return (Percent < 0.5) ? (Percent * Percent * Percent * 4) : ((Percent - 1) * (Percent - 1) * (Percent - 1) * 4 + 1);
                case 7:
                    return Mathf.Pow(2, 10 * (float)(Percent - 1));
                case 8:
                    return -Mathf.Pow(2, -10 * (float)Percent) + 1;
                case 9:
                    return (Percent < 0.5) ? (Mathf.Pow(2, 10 * (2 * (float)Percent - 1)) / 2) : ((-Mathf.Pow(2, -10 * (2 * (float)Percent - 1)) + 2) / 2);
                case 10:
                    return -Mathf.Cos((float)Percent * Mathf.PI / 2) + 1;
                case 11:
                    return Mathf.Sin((float)Percent * Mathf.PI / 2);
                case 12:
                    return (Mathf.Cos((float)Percent * Mathf.PI) - 1) / -2;
            }
            return 1;
        }
    }
}
