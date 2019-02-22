using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyRequest;
using Lanotalium.Chart;
using Lanotalium.Plugin;
using Lanotalium.Plugin.Simple.Wrapper;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

namespace Flowaria.Lanotalium.Plugin.Tweak
{
    public class AskForHeaderSetting
    {
        [Name("Show Default Header (when disable, text will be transparent to)")]
        public bool DefaultHeader = true;

        [Name("└R")]
        [Range(0.0f, 1.0f)]
        public float DefaultHeaderR = 0.392f;

        [Name("└G")]
        [Range(0.0f, 1.0f)]
        public float DefaultHeaderG = 0.392f;

        [Name("└B")]
        [Range(0.0f, 1.0f)]
        public float DefaultHeaderB = 0.392f;

        [Name("└Alpha of the Header")]
        [Range(0.0f, 1.0f)]
        public float DefaultHeaderAlpha = 1.0f;

        [Name("Use Official Lanota Style Header (this cannot be rolled-back until you load other project)")]
        public bool LanotaHeader = false;
    }

    public class AskForLanotaSetting
    {
        [Name("Alpha of the Header")]
        [Range(0.0f, 1.0f)]
        public float HeaderAlpha = 1.0f;

        [Name("Difficulty (Number should be separate by ;)")]
        public string Difficalty = "Master;15";

        [Name("Difficulty Color Type\n0: invisible, 1: whisper, 2: acoustic, 3: ultra, 4: master, 5: custom")]
        [Range(0, 5)]
        public int DifficaltyType = 4;

        [Name("└Custom Color R")]
        [Range(0.0f, 1.0f)]
        public float DifficaltyR = 1.0f;

        [Name("└Custom Color G")]
        [Range(0.0f, 1.0f)]
        public float DifficaltyG = 1.0f;

        [Name("└Custom Color B")]
        [Range(0.0f, 1.0f)]
        public float DifficaltyB = 1.0f;

        //[Name("Add Progress bar (Nintendo Switch)")]
        //public bool ProgressBar = false;
    }

    public class HeaderTweak : ILanotaliumPlugin
    {
        public string Name(Language language)
        {
            return "UiTweak - Header";
        }

        public string Description(Language language)
        {
            return "Header Customize, Tuner Alpha, Lanota Theme Header";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            if(!context.IsProjectLoaded)
            {
                context.MessageBox.ShowMessage("You must load the project first");
                
            }
            else
            {
                yield return InitResources(context);

                if (!IsLanotaThemeLoaded()) //Default skin
                {
                    Request<AskForHeaderSetting> request = new Request<AskForHeaderSetting>();
                    yield return context.UserRequest.Request(request, "Header Setting");
                    if (request.Succeed)
                    {
                        var r = request.Object;
                        if (r.LanotaHeader)
                        {
                            Header.GetComponent<Image>().color = Color.gray;
                            Request<AskForLanotaSetting> request2 = new Request<AskForLanotaSetting>();
                            yield return context.UserRequest.Request(request2, "Lanota Theme Header Setting");
                            if (request2.Succeed)
                            {
                                context.EditorManager.TunerWindow.DisplayManager.FullScreenTuner = true;
                                InitLanotaTheme(request2.Object);
                                context.EditorManager.TunerWindow.DisplayManager.FullScreenTuner = false;
                            }
                        }
                    }
                }
                else //Lanota Skin
                {
                    Request<AskForLanotaSetting> request2 = new Request<AskForLanotaSetting>();
                    yield return context.UserRequest.Request(request2, "Edit Lanota Theme Header Setting");
                    if (request2.Succeed)
                    {
                        context.EditorManager.TunerWindow.DisplayManager.FullScreenTuner = true;
                        InitLanotaTheme(request2.Object, false);
                        context.EditorManager.TunerWindow.DisplayManager.FullScreenTuner = false;
                    }
                }
                yield return null;
            }
        }

        private GameObject Header, FullScreenCanvas;
        private GameObject Copyright, ChartName, ChartDesigner, ChartDesigner_Placeholder, ChartDesigner_Text;
        public Color cWhisper, cAcoustic, cUltra, cMaster, cLanotaText;

        private Resources.LanotaHeaderResources Res = null;

        public IEnumerator InitResources(LanotaliumContext context)
        {
            //Find Objects
            FullScreenCanvas = GameObject.Find("FullScreenCanvas");
            Header = GameObject.Find("Head");
            Copyright = GameObject.Find("Copyright");
            ChartName = GameObject.Find("ChartName");
            ChartDesigner = GameObject.Find("ChartDesigner");
            ChartDesigner_Placeholder = GameObject.Find("ChartDesigner/Placeholder");
            ChartDesigner_Text = GameObject.Find("ChartDesigner/Text");

            cWhisper = new Color(0.113f, 0.294f, 0.396f); //29 75 101
            cAcoustic = new Color(0.031f, 0.470f, 0.454f); //8 120 116
            cUltra = new Color(0.494f, 0.074f, 0.062f); //126 19 16
            cMaster = new Color(0.431f, 0.141f, 0.525f); //110 36 134
            cLanotaText = new Color(0.937f, 0.917f, 0.788f); //239, 234, 201

            yield return ResourceBundle.LoadFromBundle<Resources.LanotaHeaderResources>(Application.streamingAssetsPath + "/Assets/uitweak/uitweak.header", x => Res = x);
            yield return null;
        }

        public void InitLanotaTheme(AskForLanotaSetting setting, bool isInit = true)
        {
            //Ready for Init, change Fullscreen canvas mode
            FullScreenCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

            GameObject textcolor, textlevel, textname, reff;

            //Scale
            ChartDesigner.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 275);

            //Remove Default Element
            Header.GetComponent<Image>().overrideSprite = Res.Sprite_TopBar;
            Copyright.GetComponent<Text>().color = Color.clear;

            //Font
            ChartName.GetComponent<Text>().font = Res.Font_Kawoszeh;
            ChartDesigner_Placeholder.GetComponent<Text>().font = Res.Font_Kawoszeh;
            ChartDesigner_Text.GetComponent<Text>().font = Res.Font_Kawoszeh;

            //Text Setting
            ChartName.GetComponent<Text>().fontSize = 19;
            ChartName.GetComponent<Text>().color = cLanotaText;
            ChartDesigner_Placeholder.GetComponent<Text>().color = cLanotaText;
            ChartDesigner_Text.GetComponent<Text>().color = cLanotaText;
            if(isInit)
                ChartName.GetComponent<RectTransform>().anchoredPosition += Vector2.left * -60.0F;

            //Create Prefab Object (only first) and position
            if (isInit)
            {
                reff = GameObject.Instantiate(Res.Prefab_LanotaHeader, Header.transform, false);
                reff.name = "LanotaHeader";
                reff.transform.SetAsFirstSibling();//change render order textcolor->text
            }
            
            textname = GameObject.Find("LanotaHeader/DifficultyGlow/TextName");
            textlevel = GameObject.Find("LanotaHeader/DifficultyGlow/TextLevel");
            textcolor = GameObject.Find("LanotaHeader/DifficultyGlow");

            //Change Color of TextGlow
            var nColor = Color.white;
            switch(setting.DifficaltyType)
            {
                case 0: //invisible
                    nColor = Color.clear;
                    break;
                    
                case 1:
                    nColor = cWhisper;
                    break;
                    
                case 2:
                    nColor = cAcoustic;
                    break;
                case 3:
                    nColor = cUltra;
                    break;

                case 4:
                    nColor = cMaster;
                    break;

                case 5: //custom
                    nColor = new Color(setting.DifficaltyR, setting.DifficaltyG, setting.DifficaltyB, 1.0f);
                    break;
            }
            textcolor.GetComponent<Image>().color = nColor;

            //Difficalty Name/Level
            var textdiff = setting.Difficalty.Split(';');
            if(textdiff.Length >= 2)
            {
                textname.GetComponent<Text>().text = textdiff[0];
                textlevel.GetComponent<Text>().text = textdiff[1];
            }
            else
            {
                textname.GetComponent<Text>().text = setting.Difficalty;
            }

            FullScreenCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        }

        public bool IsLanotaThemeLoaded()
        {
            return GameObject.Find("LanotaHeader") != null;
        }
    }
}
