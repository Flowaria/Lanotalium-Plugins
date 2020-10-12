using UnityEngine;

namespace Flowaria.Railnote.Curve.Lib
{
    public static class EasingLookupTable
    {
        private static ThreadsafeCurve _Curve = null;
        private static ThreadsafeEase
            T1, T2, T3,
            T4, T5, T6,
            T7, T8, T9,
            T10, T11, T12;

        static EasingLookupTable()
        {
            int sampleSize = 5000;
            T1 = new ThreadsafeEase(sampleSize, (p) => (p * p * p * p));
            T2 = new ThreadsafeEase(sampleSize, (p) => (-(p - 1) * (p - 1) * (p - 1) * (p - 1) + 1));
            T3 = new ThreadsafeEase(sampleSize, (p) => (p < 0.5) ? (p * p * p * p * 8) : ((p - 1) * (p - 1) * (p - 1) * (p - 1) * -8 + 1));

            T4 = new ThreadsafeEase(sampleSize, (p) => (p * p * p));
            T5 = new ThreadsafeEase(sampleSize, (p) => ((p - 1) * (p - 1) * (p - 1) + 1));
            T6 = new ThreadsafeEase(sampleSize, (p) => (p < 0.5) ? (p * p * p * 4) : ((p - 1) * (p - 1) * (p - 1) * 4 + 1));

            T7 = new ThreadsafeEase(sampleSize, (p) => Mathf.Pow(2, 10 * (float)(p - 1)));
            T8 = new ThreadsafeEase(sampleSize, (p) => -Mathf.Pow(2, -10 * (float)p) + 1);
            T9 = new ThreadsafeEase(sampleSize, (p) => (p < 0.5) ? (Mathf.Pow(2, 10 * (2 * (float)p - 1)) / 2) : ((-Mathf.Pow(2, -10 * (2 * (float)p - 1)) + 2) / 2));

            T10 = new ThreadsafeEase(sampleSize, (p) => -Mathf.Cos((float)p * Mathf.PI / 2) + 1);
            T11 = new ThreadsafeEase(sampleSize, (p) => Mathf.Sin((float)p * Mathf.PI / 2));
            T12 = new ThreadsafeEase(sampleSize, (p) => (Mathf.Cos((float)p * Mathf.PI) - 1) / -2);

            var curve = new AnimationCurve();
            for (int i = 0; i <= 50; ++i)
            {
                float t = 0.02f * i;
                Vector2 pt = CubicBeizer(t);
                curve.AddKey(pt.x, pt.y);
            }

            _Curve = new ThreadsafeCurve(sampleSize, curve);
        }

        private static Vector2 CubicBeizer(float t)
        {
            var Point1 = new Vector2(0.8f, 0.15f);
            var Point2 = new Vector2(0.95f, 0.1f);
            return 3 * Mathf.Pow(1 - t, 2) * t * Point1 + 3 * Mathf.Pow(t, 2) * (1 - t) * Point2 + Mathf.Pow(t, 3) * new Vector2(1, 1);
        }

        public static float MoveEasePercentEvaluate(float percent, bool demoMode)
        {
            if(demoMode)
            {
                return _Curve.Evaluate(percent * 0.01f) * 100.0f;
            }
            else
            {
                return EaseEvaluate(percent * 0.01f, 7) * 100.0f;
            }
        }

        public static float EaseEvaluate(float time, int mode)
        {
            if (time >= 1.0) return 1.0f;
            else if (time <= 0.0) return 0.0f;
            switch (mode)
            {
                case 1: return T1.Evaluate(time);
                case 2: return T2.Evaluate(time);
                case 3: return T3.Evaluate(time);

                case 4: return T4.Evaluate(time);
                case 5: return T5.Evaluate(time);
                case 6: return T6.Evaluate(time);

                case 7: return T7.Evaluate(time);
                case 8: return T8.Evaluate(time);
                case 9: return T9.Evaluate(time);

                case 10: return T10.Evaluate(time);
                case 11: return T11.Evaluate(time);
                case 12: return T12.Evaluate(time);

                default: return time;
            }
        }
    }
}