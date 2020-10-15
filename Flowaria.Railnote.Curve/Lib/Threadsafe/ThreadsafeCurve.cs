using System.Collections.Generic;
using UnityEngine;

namespace Flowaria.Railnote.Curve.Lib
{
    public sealed class ThreadsafeCurve
    {
        public int SampleCount { get; private set; } = 10;
        private readonly AnimationCurve _Curve;
        private float[] _Values = null;

        private ThreadsafeCurve()
        {
        }

        public ThreadsafeCurve(int sampleCount, AnimationCurve curve)
        {
            SampleCount = sampleCount;
            _Curve = curve;
            Cache();
        }

        public void Cache()
        {
            _Values = new float[SampleCount + 1];

            for (int i = 0; i <= SampleCount; i++)
            {
                _Values[i] = _Curve.Evaluate(i / (float)SampleCount);
            }
        }

        public float Evaluate(float time)
        {
            time = Mathf.Clamp01(time);
            return _Values[Mathf.RoundToInt(time * SampleCount)];
        }
    }
}