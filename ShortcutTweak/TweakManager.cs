using Flowaria.Lanotalium.Plugin.Tweak;
using Lanotalium.Chart;
using Lanotalium.Plugin;
using Newtonsoft.Json;
//using ShortcutTweak.Feature;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Flowaria.Lanotalium.Plugin
{
    public class EditorTweakCfg
    {
        [JsonProperty(PropertyName = "CtrlCV.UseSound")]
        public bool CV_UseSoundEffect = true;

        [JsonProperty(PropertyName = "CtrlCV.DebugCounter")]
        public bool CV_DebugCounter = false;
    }

    public class TweakManager : MonoBehaviour
    {
        public LanotaliumContext context;
        public EditorTweakCfg cfg;

        //private CopyPaster copier;

        List<InputField> InputFields = new List<InputField>();
        void Start()
        {
            foreach (GameObject obj in FindObjectsOfType(typeof(GameObject)))
            {
                InputField field = null;
                if((field = obj.GetComponent<InputField>()) != null)
                {
                    InputFields.Add(field);
                }
            }
            //context.EditorManager.InspectorWindow.ComponentBpm.BeatlineText.gameObject.AddComponent<BeatlineButtonManager>();
            //context.EditorManager.InspectorWindow.ComponentBpm.BeatlineText.gameObject.GetComponent<BeatlineButtonManager>().context = context;
            StartCoroutine("UpdateCopyPaster");
            StartCoroutine("UpdateInputFieldParameter");
            StartCoroutine("UpdateHotKey");
        }


        CopyPaster cp;
        IEnumerator UpdateCopyPaster()
        {
            cp = new CopyPaster(context, cfg);
            while (true)
            {
                if (!hasFocusedField())
                {
                    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.C))
                    {
                        yield return cp.CopyAll();
                        yield return new WaitForSeconds(0.3f);
                    }
                    //Paste
                    else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.V))
                    {
                        yield return cp.PasteAll(context.EditorManager.MusicPlayerWindow.CurrentTime);
                        yield return new WaitForSecondsRealtime(0.75f);
                    }
                    //Special Paste
                    else if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.V))
                    {
                        yield return cp.ShowSpecialPaste();
                        yield return new WaitForSecondsRealtime(0.75f);
                    }
                }
                yield return null;
            }
        }

        IEnumerator UpdateInputFieldParameter()
        {
            while(true)
            {
                foreach (var field in InputFields)
                {
                    if (field.isFocused)
                    {
                        field.text = field.text.Replace("c", context.TunerManager.ChartTime.ToString());

                        if(context.EditorManager.InspectorWindow.ComponentBpm.EnableBeatline)
                        {
                            float beatline = (60.0f / context.TunerManager.BpmManager.CurrentBpm) / context.EditorManager.InspectorWindow.ComponentBpm.BeatlineDensity;
                            field.text = field.text.Replace("b", beatline.ToString());
                            field.text = field.text.Replace("n", context.OperationManager.FindNearestBeatlineByTime(context.TunerManager.ChartTime).ToString());
                        }
                        
                    }
                }
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        IEnumerator UpdateHotKey()
        {
            while (true)
            {
                //Create Tap
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.T))
                {
                    LanotaTapNote NewNote = new LanotaTapNote
                    {
                        Time = context.TunerManager.ChartTime,
                        Degree = 0,
                        Size = 1
                    };
                    context.OperationManager.AddTapNote(NewNote, false);
                    yield return new WaitForSeconds(0.3f);
                }
                //Create Hold
                else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.H))
                {
                    LanotaHoldNote NewNote = new LanotaHoldNote
                    {
                        Time = context.TunerManager.ChartTime,
                        Degree = 0,
                        Duration = 1.0f,
                        Size = 1
                    };
                    context.OperationManager.AddHoldNote(NewNote, false);
                    yield return new WaitForSecondsRealtime(0.3f);
                }
                //Create Bpm
                else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.B))
                {
                    LanotaChangeBpm NewBpm = new LanotaChangeBpm
                    {
                        Time = context.TunerManager.ChartTime,
                        Bpm = 100.0f
                    };
                    context.OperationManager.AddBpm(NewBpm, false);
                    yield return new WaitForSecondsRealtime(0.3f);
                }
                //Create Scroll Speed
                else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.F))
                {
                    LanotaScroll NewSpeed = new LanotaScroll
                    {
                        Time = context.TunerManager.ChartTime,
                        Speed = 1.0f
                    };
                    context.OperationManager.AddScrollSpeed(NewSpeed, false);
                    yield return new WaitForSecondsRealtime(0.3f);
                }
                //Enable Click to Create
                else if(Input.GetKey(KeyCode.F2))
                {
                    context.EditorManager.CreatorWindow.ClickToCreateManager.Enable = !context.EditorManager.CreatorWindow.ClickToCreateManager.Enable;
                    yield return new WaitForSecondsRealtime(0.4f);
                }

                yield return null;

                //Click to Create snap hotkey
                /* //I'm note sure
                if(context.EditorManager.CreatorWindow.ClickToCreateManager.Enable)
                {
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        context.EditorManager.CreatorWindow.ClickToCreateManager.AttachToAngleline = !context.EditorManager.CreatorWindow.ClickToCreateManager.AttachToAngleline;
                        yield return new WaitForSecondsRealtime(0.4f);
                    }
                    if (Input.GetKey(KeyCode.LeftAlt))
                    {
                        context.EditorManager.CreatorWindow.ClickToCreateManager.AttachToBeatline = !context.EditorManager.CreatorWindow.ClickToCreateManager.AttachToBeatline;
                        yield return new WaitForSecondsRealtime(0.4f);
                    }
                }
                */
            }
        }

        bool hasFocusedField()
        {
            return InputFields.Exists(x=>x.isFocused == true);
        }
    }
}
