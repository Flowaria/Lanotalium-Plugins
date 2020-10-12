using System;
using System.Collections.Generic;
using UnityEngine;

namespace Flowaria.Railnote.Curve.Lib
{
    public class ThreadsafeEase
    {
        public int SampleCount { get; private set; } = 10;
        private Dictionary<long, float> _Values = new Dictionary<long, float>();

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
            _Values.Clear();

            for (int i = 0; i <= SampleCount; i++)
            {
                _Values.Add(i, evalFunc.Invoke(i / (float)SampleCount));
            }
        }

        public float Evaluate(float time)
        {
            time = Mathf.Clamp01(time);
            return _Values[Mathf.RoundToInt(time * SampleCount)];
        }
    }
}