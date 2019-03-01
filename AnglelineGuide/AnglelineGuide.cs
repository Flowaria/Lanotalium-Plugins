using EasyRequest;
using Lanotalium.Plugin;
using Lanotalium.Plugin.Simple.Wrapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Flowaria.Lanotalium.Plugin
{
    public class AskForGuide
    {
        [Name("Guide String (Work same as Angleline) / Or Empty it for disable")]
        public string GuideString = "0>360/8";

        [Name("Text is in outside of Tuner?")]
        public bool OutText = true;
    }

    public class AnglelineResources
    {
        [ResourceName("OutTextGuide")]
        public GameObject Prefab_OutTextGuide;

        [ResourceName("InTextGuide")]
        public GameObject Prefab_InTextGuide;
    }

    public class AnglelineGuide : ILanotaliumPlugin
    {
        public string Description(Language language)
        {
            return "Angleline Guide";
        }

        public string Name(Language language)
        {
            return "Angleline Guide";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            if (context.IsProjectLoaded)
            {
                Request<AskForGuide> request = new Request<AskForGuide>();
                yield return context.UserRequest.Request(request, "Guide Setting");
                if (request.Succeed)
                {
                    GameObject i = null;
                    AnglelineGuideManager mgr = null;
                    AnglelineResources r = null;
                    yield return ResourceBundle.LoadFromBundle<AnglelineResources>(Application.streamingAssetsPath + "/Assets/anglelineguide", x => r = x);
                    if ((i = GameObject.Find("AnglelineGuideManagerObject")) == null)
                    {
                        i = new GameObject();
                        i.name = "AnglelineGuideManagerObject";
                        i.transform.SetParent(GameObject.Find("Tuner").transform);
                        mgr = i.AddComponent<AnglelineGuideManager>();
                        mgr.Tuner = context.TunerManager;
                        mgr.TunerObject = GameObject.Find("Tuner");
                        mgr.InTextGuide = r.Prefab_InTextGuide;
                        mgr.OutTextGuide = r.Prefab_OutTextGuide;
                        
                    }
                    else
                    {
                        mgr = i.GetComponent<AnglelineGuideManager>();
                    }
                    try
                    {
                        mgr.SetFormatText(request.Object.GuideString, request.Object.OutText);
                    }
                    catch(FormatException e)
                    {
                        context.MessageBox.ShowMessage("There is something wrong in format string.\nReason: "+e.Message);
                    }
                    catch (DivideByZeroException)
                    {
                        context.MessageBox.ShowMessage("There is something wrong in format string.\nReason: Can not divide by Zero");
                    }
                }
            }
        }
    }

    public class AnglelineGuideManager : MonoBehaviour
    {
        public GameObject InTextGuide, OutTextGuide, TunerObject;
        public LimTunerManager Tuner;
        public AnglelineGuideManager()
        {
            
        }

        public void SetFormatText(string text, bool outtext)
        {
            Clear();
            if(text != "")
            {
                var eachFormat = text.Split(';');
                if (eachFormat.Length == 1)
                {
                    foreach (var child in eachFormat)
                    {
                        foreach (var degree in SingleFormatToList(child))
                        {
                            Add(outtext, degree);
                        }
                    }
                }
                else if (eachFormat.Length > 1)
                {
                    foreach (var degree in SingleFormatToList(text))
                    {
                        Add(outtext, degree);
                    }
                }
            }
        }

        private List<float> SingleFormatToList(string str)
        {
            if(str.Contains('>'))
            {
                if (str.Contains('/'))
                {
                    float start, end, divide;
                    var sp1 = str.Split('>');
                    if(sp1.Length == 2 && float.TryParse(sp1[0], out start))
                    {
                        var sp2 = sp1[1].Split('/');
                        if(sp2.Length == 2
                            && float.TryParse(sp2[0], out end)
                            && float.TryParse(sp2[1], out divide))
                        {
                            if(start < end)
                            {
                                if (divide != 0.0f)
                                {
                                    float delta = (end - start) / divide;
                                    var list = new List<float>();
                                    for(;start < end;start += delta)
                                    {
                                        list.Add(start);
                                    }
                                    return list;
                                }
                                else
                                    throw new DivideByZeroException();
                            }
                            else
                                throw new FormatException("Start Degree is bigger then End Degree");
                        }
                        else
                            throw new FormatException("End Degree or Divide is wrong");
                    }
                    else
                        throw new FormatException("Start Degree is wrong");
                }
                else
                    throw new FormatException("Format is wrong");
            }
            else if(!str.Contains('/'))
            {
                if (float.TryParse(str, out float result))
                {
                    return new List<float>(new float[] { result });
                }
                else
                    throw new FormatException("This is not a number");
            }
            else
                throw new FormatException("Format is wrong");
        }

        private void Add(bool outText, float degree)
        {
            GameObject obj = null;
            if(outText)
            {
                obj = GameObject.Instantiate(OutTextGuide, transform, false);
            }
            else
            {
                obj = GameObject.Instantiate(InTextGuide, transform, false);
            }
            obj.transform.Rotate(new Vector3(0, degree, 0));

            var comp = obj.AddComponent<TextKeepDegree>();
            comp.Tuner = Tuner;
            comp.Text = obj.transform.Find("Text").gameObject;
            comp.Degree = degree;
        }

        public void Clear()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public class TextKeepDegree : MonoBehaviour
    {
        public GameObject Text;
        public LimTunerManager Tuner;
        public float Degree;

        public void Start()
        {
            Text.GetComponent<TextMesh>().text = Degree.ToString("0.00");
        }

        public void Update()
        {
            Text.transform.localEulerAngles = new Vector3(-90, 180 - Degree - Tuner.CameraManager.CurrentRotation, 0);
        }
    }
}
