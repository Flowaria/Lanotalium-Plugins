using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Flowaria.Railnote.Curve.Lib
{

    //TODO: NO FOR NOW
    public struct HoldLineWorkerMovePercentJob : IJobParallelFor
    {
        public float percentStep;
        public float startTime, deltaTime;

        public float currentTime;
        public float scrollConst;
        public float playSpeed;

        [ReadOnly] public NativeArray<float> scrollTimes;
        [ReadOnly] public NativeArray<float> scrollSpeeds;
        public NativeArray<float> movePercents;
        

        public void Execute(int i)
        {
            float percent = i * percentStep;
            float time = startTime + (deltaTime * percent);

            if (time <= currentTime)
            {
                movePercents[i] = 100.0f;
            }
            else
            {
                movePercents[i] = CalculateMovePercent(time);
            }
        }

        private float CalculateMovePercent(float time)
        {
            int scrollCount = scrollTimes.Length;

            int StartScroll = 0, EndScroll = 0;
            float Percent = 100;

            for (int i = 0; i < scrollCount - 1; i++)
            {
                if (currentTime >= scrollTimes[i] && currentTime < scrollTimes[i + 1])
                    StartScroll = i;
                if (time >= scrollTimes[i] && time < scrollTimes[i + 1])
                    EndScroll = i;
            }

            if (scrollCount != 0)
            {
                if (currentTime >= scrollTimes[scrollCount - 1])
                    StartScroll = scrollCount - 1;

                if (time >= scrollTimes[scrollCount - 1])
                    EndScroll = scrollCount - 1;
            }

            for (int i = StartScroll; i <= EndScroll; i++)
            {
                if (StartScroll == EndScroll)
                {
                    Percent -= (time - currentTime) * scrollSpeeds[i] * scrollConst * playSpeed;
                }
                else if (StartScroll != EndScroll)
                {
                    if (i == StartScroll)
                        Percent -= (scrollTimes[i + 1] - currentTime) * scrollSpeeds[i] * scrollConst * playSpeed;

                    else if (i != EndScroll && i != StartScroll)
                        Percent -= (scrollTimes[i + 1] - scrollTimes[i]) * scrollSpeeds[i] * scrollConst * playSpeed;

                    else if (i == EndScroll)
                        Percent -= (time - scrollTimes[i]) * scrollSpeeds[i] * scrollConst * playSpeed;
                }
            }

            Percent = Mathf.Clamp(Percent, 0, 100);
            return Percent;
        }
    }
}
