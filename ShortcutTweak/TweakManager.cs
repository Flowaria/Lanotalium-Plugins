using Lanotalium.Plugin;
//using ShortcutTweak.Feature;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ShortcutTweak
{
    public class TweakManager : MonoBehaviour
    {
        public LanotaliumContext context;

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

            StartCoroutine("UpdateCopyPaster");
            StartCoroutine("UpdateInputFieldParameter");
        }

        IEnumerator UpdateCopyPaster()
        {
            while (true)
            {
                if (!hasFocusedField())
                {
                    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.C))
                    {
                        //yield return CopyAll(context);
                        yield return new WaitForSeconds(0.3f);
                    }
                    //Paste
                    else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.V))
                    {
                        //yield return PasteAll(context, context.EditorManager.MusicPlayerWindow.CurrentTime);
                        yield return new WaitForSecondsRealtime(0.75f);
                    }
                    //Special Paste
                    else if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.V))
                    {
                        //yield return ShowSpecialPaste(context);
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
                        field.text = field.text.Replace("[!current]", context.TunerManager.ChartTime.ToString());

                        if(context.EditorManager.InspectorWindow.ComponentBpm.EnableBeatline)
                        {
                            float beatline = (60.0f / context.TunerManager.BpmManager.CurrentBpm) / context.EditorManager.InspectorWindow.ComponentBpm.BeatlineDensity;
                            field.text = field.text.Replace("[!beat]", beatline.ToString());
                            field.text = field.text.Replace("[!nearbeat]", context.OperationManager.FindNearestBeatlineByTime(context.TunerManager.ChartTime).ToString());
                        }
                        
                    }
                }
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        bool hasFocusedField()
        {
            return InputFields.Exists(x=>x.isFocused == true);
        }
    }
}
