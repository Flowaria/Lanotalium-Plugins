using System.Collections.Generic;
using UnityEngine;

namespace Flowaria.Railnote.Curve.Lib
{
    public class ThreadsafeCurve
    {
        public int SampleCount { get; private set; } = 10;
        public AnimationCurve Curve;
        private Dictionary<long, float> _Values = new Dictionary<long, float>();

        private ThreadsafeCurve()
        {
        }

        public ThreadsafeCurve(int sampleCount, AnimationCurve curve)
        {
            SampleCount = sampleCount;
            Curve = curve;
            Cache();
        }

        public void Cache()
        {
            _Values.Clear();

            for (int i = 0; i <= SampleCount; i++)
            {
                _Values.Add(i, Curve.Evaluate(i / (float)SampleCount));
            }
        }

        public float Evaluate(float time)
        {
            time = Mathf.Clamp01(time);
            return _Values[Mathf.RoundToInt(time * SampleCount)];
        }
    }
}