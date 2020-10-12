using EasyRequest;
using Lanotalium.Chart;
using Lanotalium.Plugin;
using Lanotalium.Plugin.Simple;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Flowaria.Lanotalium.Plugin
{
    public class BetterCatchRail : ILanotaliumPlugin
    {
        public string Name(Language language)
        {
            return "Create Catch Rail (adv)";
        }

        public string Description(Language language)
        {
            return "Create Catch Rail but More Improved";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            if(!context.IsProjectLoaded)
            {
                yield break;
            }

            if (context.OperationManager.SelectedTapNote.Count != 2)
            {
                if (context.OperationManager.SelectedTapNote.Count > 2)
                {
                    context.MessageBox.ShowMessage("More then 2 Tap note selected");
                }
                else
                {
                    context.MessageBox.ShowMessage("Please Select 2 Click / Catch note");
                }

                yield break;
            }

            Request<AskCatchRail> r = new Request<AskCatchRail>();
            yield return context.UserRequest.Request(r, "Create Catch Rail (Advanced)");
            if (r.Succeed)
            {
                var p = r.Object;
                var o = context.OperationManager;
                var rand = new System.Random();

                LanotaTapNote first = null, last = null;

                if (o.SelectedTapNote.First().Time < o.SelectedTapNote.Last().Time)
                {
                    first = o.SelectedTapNote.First();
                    last = o.SelectedTapNote.Last();
                }
                else
                {
                    last = o.SelectedTapNote.First();
                    first = o.SelectedTapNote.Last();
                }

                var count = p.Count;
                if (count == 0)
                {
                    var bpms = o.InspectorManager.ComponentBpm;
                    bpms.EnableBeatline = true;

                    var firstIdx = o.FindNearestBeatlineIndexByTime(first.Time);
                    var lastIdx = o.FindNearestBeatlineIndexByTime(last.Time);

                    count = lastIdx - firstIdx - 1;

                    if (count > 99999)
                    {
                        context.MessageBox.ShowMessage("Infinite Loop Detected!\nTry again");
                        yield break;
                    }

                    /*
                    var beat = first.Time;
                    while (beat < last.Time)
                    {
                        if(count > 99999)
                        {
                            context.MessageBox.ShowMessage("Infinite Loop Detected!\nTry again");
                            yield break;
                        }

                        beat = o.InspectorManager.ComponentBpm.FindPrevOrNextBeatline(beat, true);
                        count++;
                    }
                    count--;
                    */
                }

                if (count <= 0)
                {
                    context.MessageBox.ShowMessage("There is no Beatline Between Two Selected note");
                    yield break;
                }

                var deltaTime = (last.Time - first.Time) / (count + 1);
                var firstDegMod = first.Degree % 360.0f;
                var lastDegMod = last.Degree % 360.0f;

                var firstDegAbs = firstDegMod > 0.0f ? firstDegMod : 360 - (-firstDegMod);
                var lastDegAbs = lastDegMod > 0.0f ? lastDegMod : 360 - (-lastDegMod);
                var deltaDegree = (lastDegAbs) - (firstDegAbs);

                //Clockwise but delta is Anti-clockwise
                if (p.Clockwise && deltaDegree > 0.0f)
                {
                    deltaDegree = 360.0f - deltaDegree;
                }
                //Anti-clockwise but delta is Clockwise
                else if (!p.Clockwise && deltaDegree < 0.0f)
                {
                    deltaDegree = 360.0f + deltaDegree;
                }

                if(p.Revolution > 0)
                {
                    if (p.Clockwise)
                    {
                        deltaDegree -= p.Revolution * 360.0f;
                    }

                    else
                    {
                        deltaDegree += p.Revolution * 360.0f;
                    }  
                }

                var sizePattern = new IntPattern(ValidSizeAndTypePattern);
                var typePattern = new IntPattern(ValidSizeAndTypePattern);
                if (sizePattern.ReadPattern(p.SizePattern) && typePattern.ReadPattern(p.TypePattern))
                {
                    for (int i = 0; i < count; i++)
                    {
                        var percent = (float)(i + 1) / (float)(count + 1);
                        var note = new LanotaTapNote()
                        {
                            Time = first.Time + (deltaTime * (i + 1)),
                            Degree = first.Degree + deltaDegree * Ease.CalculateEase(percent, p.Ease),
                            @Type = ConvertNoteType(typePattern.GetValue(i)),
                            Size = sizePattern.GetValue(i),
                        };
                        if (p.Random != 0.0f)
                        {
                            var randDegree = ((float)rand.NextDouble() - 0.5f) * 2.0f * p.Random;
                            note.Degree += randDegree;
                        }
                        o.AddTapNote(note);
                    }
                }
                else
                {
                    context.MessageBox.ShowMessage("Error in Pattern String!");
                }
            }
        }

        public bool ValidSizeAndTypePattern(int input)
        {
            return input >= 0 && input <= 3;
        }

        public int ConvertNoteType(int type)
        {
            if (type == 0)
                return 0;
            if (type == 1)
                return 4;
            if (type == 2)
                return 2;
            if (type == 3)
                return 3;
            return 0;
        }
    }

    public class IntPattern
    {
        public Func<int,bool> Validation = null;

        public IntPattern(Func<int,bool> validation)
        {
            Validation = validation;
        }

        private List<int> Pattern = new List<int>();
        public bool ReadPattern(string pattern)
        {
            for(int i = 0; i < pattern.Count(); i++)
            {
                var result = int.TryParse(pattern[i].ToString(), out int value);
                if (result)
                {
                    if (Validation != null)
                    {
                        if (Validation(value))
                            Pattern.Add(value);
                        else
                            return false;
                    }
                    else
                    {
                        Pattern.Add(value);
                    }
                }
                else
                    return false;
            }
            return true;
        }

        public int GetValue(int index)
        {
            return Pattern[index & Pattern.Count-1];
        }
    }

    public class AskCatchRail
    {
        [Name("Note Count (Except First and End note And 0 to fit to beatline setting)")]
        [Range(0, 99999)]
        public int Count = 0;

        [Name("Degree Ease")]
        [Range(0, 12)]
        public int Ease = 0;

        [Name("Type Variation Pattern (0: click, 1: catch, 2: flick in, 3: flick out)")]
        public string TypePattern = "1";

        [Name("Size Variation Pattern")]
        public string SizePattern = "1";

        [Name("Apply Random Degree (0: disable)")]
        [Range(0.0f, 999999.0f)]
        public float Random = 0.0f;

        [Name("Revolution Count")]
        [Range(0, 999999999)]
        public int Revolution = 0;

        [Name("Clockwise")]
        public bool Clockwise = true;
    }
}
