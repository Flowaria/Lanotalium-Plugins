using System;
using System.Collections.Generic;
using UnityEngine;

namespace Flowaria.Railnote.Curve.Lib
{
    public sealed class ThreadsafeEase
    {
        public int SampleCount { get; private set; } = 10;
        private float[] _ValuesF;

        private ThreadsafeEase()
        {
        }

        public ThreadsafeEase(int sampleCount, Func<float, float> evalFunc)
        {
            SampleCount = sampleCount;
            Cache(sampleCount, evalFunc);
        }

        private void Cache(int sample, Func<float, float> evalFunc)
        {
            _ValuesF = new float[sample + 1];

            for (int i = 0; i <= SampleCount; i++)
            {
                _ValuesF[i] = evalFunc.Invoke(i / (float)SampleCount);
            }
        }

        public float Evaluate(float time)
        {
            time = Mathf.Clamp01(time);
            return _ValuesF[Mathf.RoundToInt(time * SampleCount)];
        }
    }
}