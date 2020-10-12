using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PATCH_LanotaCurvedRail.Content
{
    public class HoldNoteManagerPatch
    {
        public void PatchCaller()
        {
            UpdateAllLineRenderers_new();
        }

        public static void UpdateAllLineRenderers_new()
        {
            var tunerManager = LimTunerManager.Instance;
            var holdManager = LimTunerManager.Instance.HoldNoteManager;

            foreach (Lanotalium.Chart.LanotaHoldNote Note in holdManager.HoldNote)
            {
                if (!Note.shouldUpdate)
                {
                    continue;
                }
                if (Note.Percent < 10) continue;

                Debug.Log(Note.Time);

                /*
                if (Note.Jcount == 0)
                {
                    float Rotation = Note.Degree + tunerManager.CameraManager.CurrentRotation;
                    float EndPercent = CalculateMovePercent(Note.Time + Note.Duration);
                    Note.LineRenderer.positionCount = 10;
                    Note.LineRenderer.startWidth = Note.Percent / 100 + (Note.OnTouch ? OnTouchWidthAdd : 0);
                    Note.LineRenderer.endWidth = CalculateEasedPercent(EndPercent) / 100 + (Note.OnTouch ? OnTouchWidthAdd : 0);
                    Vector3 Start = CalculateLineRendererPoint(Note.Percent, Rotation);
                    Vector3 End = CalculateLineRendererPoint(CalculateEasedPercent(EndPercent), Rotation);
                    Vector3 Delta = (End - Start) / 9;
                    for (int i = 0; i < 10; ++i) Note.LineRenderer.SetPosition(i, Start + i * Delta);
                }
                else
                {
                    float lastPercent = 0;
                    int positionIndex = 0;
                    float currentaTime = Note.Time, currentaDegree = Note.Degree;
                    bool headPointAdded = false;

                    for (int i = 0; i < Note.Joints.Count; ++i)
                    {
                        Lanotalium.Chart.LanotaJoints Joint = Note.Joints[i];
                        if (Joint.aTime < tunerManager.ChartTime)
                        {
                            currentaTime = Joint.aTime;
                            currentaDegree = Joint.aDegree;
                            continue;
                        }

                        if (!headPointAdded)
                        {
                            if (Note.Time < tunerManager.ChartTime)
                            {
                                float startDegreePercent = (tunerManager.ChartTime - currentaTime) / Joint.dTime;
                                AddLineRendererPosition(positionIndex, Note.LineRenderer, CalculateLineRendererPoint(100, currentaDegree + Joint.dDegree * CalculateEasedCurve(startDegreePercent, Joint.Cfmi) + tunerManager.CameraManager.CurrentRotation));
                                positionIndex++;
                            }
                            headPointAdded = true;
                        }

                        int jCount = Mathf.Max(Mathf.FloorToInt(Mathf.Abs(Joint.dDegree)), 50);
                        for (int j = 0; j < jCount; ++j)
                        {
                            float degreePercent = 1f * j / (jCount - 1);
                            float degree = currentaDegree + Joint.dDegree * degreePercent;
                            float timingPercent = CalculateReverseEasedCurve(degreePercent, Joint.Cfmi);
                            float timing = currentaTime + Joint.dTime * timingPercent;
                            float percent = CalculateEasedPercent(CalculateMovePercent(timing));
                            lastPercent = percent;
                            if (percent == 100 && timing <= tunerManager.ChartTime) continue;
                            AddLineRendererPosition(positionIndex, Note.LineRenderer, CalculateLineRendererPoint(percent, degree + tunerManager.CameraManager.CurrentRotation));
                            positionIndex++;
                            if (percent <= 15) goto end;
                        }

                        currentaTime = Joint.aTime;
                        currentaDegree = Joint.aDegree;
                    }
                end:

                    Note.LineRenderer.startWidth = Note.Percent / 100;
                    Note.LineRenderer.endWidth = lastPercent / 100;
                    Note.LineRenderer.positionCount = positionIndex;
                }
                */
            }
        }

        private float CalculateEasedPercent(float Percent)
        {
            return LimNoteEase.Instance.CalculateEasedPercent(Percent);
        }

        private float CalculateMovePercent(float JudgeTime)
        {
            var tunerManager = LimTunerManager.Instance;

            int StartScroll = 0, EndScroll = 0;
            float Percent = 100;
            List<Lanotalium.Chart.LanotaScroll> Scroll = tunerManager.ScrollManager.Scroll;
            int count = Scroll.Count;
            for (int i = 0; i < count - 1; ++i)
            {
                if (Scroll[i + 1].Time < tunerManager.ChartTime) continue;
                if (tunerManager.ChartTime >= Scroll[i].Time && tunerManager.ChartTime < Scroll[i + 1].Time) StartScroll = i;
                if (Scroll[i + 1].Time < JudgeTime) continue;
                if (JudgeTime >= Scroll[i].Time && JudgeTime < Scroll[i + 1].Time) EndScroll = i;
            }
            if (count != 0)
            {
                if (tunerManager.ChartTime >= Scroll[count - 1].Time) StartScroll = count - 1;
                if (JudgeTime >= Scroll[count - 1].Time) EndScroll = count - 1;
            }
            for (int i = StartScroll; i <= EndScroll; ++i)
            {
                if (StartScroll == EndScroll) Percent -= (JudgeTime - tunerManager.ChartTime) * Scroll[i].Speed * 10 * tunerManager.ChartPlaySpeed;
                else if (StartScroll != EndScroll)
                {
                    if (i == StartScroll) Percent -= (Scroll[i + 1].Time - tunerManager.ChartTime) * Scroll[i].Speed * 10 * tunerManager.ChartPlaySpeed;
                    else if (i != EndScroll && i != StartScroll) Percent -= (Scroll[i + 1].Time - Scroll[i].Time) * Scroll[i].Speed * 10 * tunerManager.ChartPlaySpeed;
                    else if (i == EndScroll) Percent -= (JudgeTime - Scroll[i].Time) * Scroll[i].Speed * 10 * tunerManager.ChartPlaySpeed;
                }
            }
            Percent = Mathf.Clamp(Percent, 0, 100);
            return Percent;
        }

        private float CalculateReverseEasedCurve(float Percent, int Mode)
        {
            if (Percent >= 1.0) return 1.0f;
            else if (Percent <= 0.0) return 0.0f;
            switch (Mode)
            {
                case 0:
                    return Percent;
                case 1:
                    return Mathf.Pow(Percent, 0.25f);
                case 2:
                    return 1 - Mathf.Pow(1 - Percent, 0.25f);
                case 3:
                    return (Percent < 0.5) ? Mathf.Pow(Percent * 0.125f, 0.25f) : 1 - Mathf.Pow((1 - Percent) * 0.125f, 0.25f);
                case 4:
                    return Mathf.Pow(Percent, 0.3333333f);
                case 5:
                    return 1 - Mathf.Pow(1 - Percent, 0.3333333f);
                case 6:
                    return (Percent < 0.5) ? Mathf.Pow(Percent * 0.25f, 0.3333333f) : 1 - Mathf.Pow((1 - Percent) * 0.25f, 0.3333333f);
                case 7:
                    return Mathf.Log(2 * Mathf.Pow(Percent, 0.1f), 2);
                case 8:
                    return Mathf.Log(Mathf.Pow(1 / (1 - Percent), 0.1f), 2);
                case 9:
                    return (Percent < 0.5) ? Mathf.Log(1.464086f * Mathf.Pow(Percent, 0.05f), 2) : Mathf.Log(1.366040f * Mathf.Pow(1 / (1 - Percent), 0.05f), 2);
                case 10:
                    return Mathf.Acos(1 - Percent) * 0.6366198f;
                case 11:
                    return Mathf.Asin(Percent) * 0.6366198f;
                case 12:
                    return Mathf.Acos(1 - 2 * Percent) * 0.3183099f;
            }
            return 1;
        }
    }
}
