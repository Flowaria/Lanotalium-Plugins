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
using UnityEngine;
using UnityEngine.UI;

namespace Flowaria.Lanotalium.Plugin
{
    public partial class UiCustomizer : ILanotaliumPlugin
    {
        public IEnumerator Process(LanotaliumContext context)
        {
            if(!context.IsProjectLoaded)
            {
                context.MessageBox.ShowMessage("You must load the project first");
            }
            else
            {
                yield return InitResources(context);

                Request<Setting> request = new Request<Setting>();
                yield return context.UserRequest.Request(request, "Ui Setting");

                if (request.Succeed)
                {
                    var r = request.Object;
                    if (r.LanotaHeader)
                    {
                        if(!IsLanotaThemeLoaded())
                        {
                            Header.GetComponent<Image>().color = Color.gray;
                            Request<LanotaThemeSetting> request2 = new Request<LanotaThemeSetting>();
                            yield return context.UserRequest.Request(request2, "Lanota Theme Setting");
                            if (request2.Succeed)
                            {
                                context.EditorManager.TunerWindow.DisplayManager.FullScreenTuner = true;
                                InitLanotaTheme(request2.Object);
                                context.EditorManager.TunerWindow.DisplayManager.FullScreenTuner = false;
                            }
                        }
                        else
                        {
                            Header.GetComponent<Image>().color = Color.gray;
                            Request<LanotaThemeSetting> request2 = new Request<LanotaThemeSetting>();
                            yield return context.UserRequest.Request(request2, "Edit Lanota Theme Setting");
                            if (request2.Succeed)
                            {
                                context.EditorManager.TunerWindow.DisplayManager.FullScreenTuner = true;
                                EditLanotaTheme(request2.Object);
                                context.EditorManager.TunerWindow.DisplayManager.FullScreenTuner = false;
                            }
                        }
                    }
                    else
                    {
                        if (!r.DefaultHeader)
                        {
                            var transparent = new Color(0, 0, 0, 0);
                            Copyright.GetComponent<Text>().color = transparent;
                            ChartName.GetComponent<Text>().color = transparent;
                            ChartDesigner_Placeholder.GetComponent<Text>().color = transparent;
                            ChartDesigner_Text.GetComponent<Text>().color = transparent;
                        }
                        else
                        {
                            Copyright.GetComponent<Text>().color = Color.white;
                            ChartName.GetComponent<Text>().color = Color.white;
                            ChartDesigner_Placeholder.GetComponent<Text>().color = Color.white;
                            ChartDesigner_Text.GetComponent<Text>().color = Color.white;
                        }
                        Header.GetComponent<Image>().color = new Color(r.DefaultHeaderR, r.DefaultHeaderG, r.DefaultHeaderB, r.DefaultHeader ? r.DefaultHeaderAlpha : 0.0f);
                        TunerBackground.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, request.Object.Alpha);
                    }
                }
                yield return null;
            }
            if (Bundle != null)
                Bundle.Unload(true);
        }

        private AssetBundle Bundle;
        private GameObject Header, TunerBackground, FullScreenCanvas;
        private GameObject Copyright, ChartName, ChartDesigner, ChartDesigner_Placeholder, ChartDesigner_Text;
        private GameObject PrefabPauseButton, PrefabTextColor;
        private Color cWhisper, cAcoustic, cUltra, cMaster, cLanotaText;

        private Sprite TopBarSprite;
        private Font Kawoszeh, Parabola, DefaultFont;

        public IEnumerator InitResources(LanotaliumContext context)
        {
            //Find Objects
            FullScreenCanvas = GameObject.Find("FullScreenCanvas");
            Header = GameObject.Find("Head");
            TunerBackground = GameObject.Find("Tuner/Background");
            Copyright = GameObject.Find("Copyright");
            ChartName = GameObject.Find("ChartName");
            ChartDesigner = GameObject.Find("ChartDesigner");
            ChartDesigner_Placeholder = GameObject.Find("ChartDesigner/Placeholder");
            ChartDesigner_Text = GameObject.Find("ChartDesigner/Text");

            //Color
            cWhisper = new Color(0.113f, 0.294f, 0.396f); //29 75 101
            cAcoustic = new Color(0.031f, 0.470f, 0.454f); //8 120 116
            cUltra = new Color(0.494f, 0.074f, 0.062f); //126 19 16
            cMaster = new Color(0.431f, 0.141f, 0.525f); //110 36 134
            cLanotaText = new Color(0.937f, 0.917f, 0.788f); //239, 234, 201

            var Bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "Assets/lanotadefui"));
            if(Bundle != null)
            {
                TopBarSprite = Bundle.LoadAsset<Sprite>("Assets/Sprites/TopBar.png");
                Kawoszeh = Bundle.LoadAsset<Font>("Assets/Font/kawoszeh.ttf");
                Parabola = Bundle.LoadAsset<Font>("Assets/Font/parabola.ttf");
                PrefabPauseButton = Bundle.LoadAsset<GameObject>("Assets/Prefabs/PauseButton.prefab");
                PrefabTextColor = Bundle.LoadAsset<GameObject>("Assets/Prefabs/DiffTextColor.prefab");
            }
            yield return null;
        }

        public void InitLanotaTheme(LanotaThemeSetting setting)
        {
            //Ready for Init, change Fullscreen canvas mode
            Header.GetComponent<Image>().overrideSprite = TopBarSprite;
            FullScreenCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

            //Setting Text
            if(setting.LanotaFont)
            {
                Copyright.GetComponent<Text>().font = Parabola;
                ChartName.GetComponent<Text>().font = Kawoszeh;
                ChartDesigner_Placeholder.GetComponent<Text>().font = Kawoszeh;
                ChartDesigner_Text.GetComponent<Text>().font = Kawoszeh;
            }
            ChartDesigner.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 275);
            //ChartName.GetComponent<RectTransform>().position += Vector3.left * -120.0F;
            ChartName.GetComponent<RectTransform>().anchoredPosition += Vector2.left * -60.0F;
            Copyright.GetComponent<RectTransform>().anchoredPosition += Vector2.left * -150F;
            Copyright.GetComponent<Text>().text = setting.Difficalty;
            Copyright.GetComponent<Text>().fontSize = 18;
            ChartName.GetComponent<Text>().fontSize = 19;

            //Color
            Copyright.GetComponent<Text>().color = cLanotaText;
            ChartName.GetComponent<Text>().color = cLanotaText;
            ChartDesigner_Placeholder.GetComponent<Text>().color = cLanotaText;
            ChartDesigner_Text.GetComponent<Text>().color = cLanotaText;

            //Create Prefab Object
            var pausebtn = GameObject.Instantiate(PrefabPauseButton, Header.transform, false);
            var textcolor = GameObject.Instantiate(PrefabTextColor, Header.transform, false);
            pausebtn.name = "PauseButton";
            textcolor.name = "DiffTextColor";

            textcolor.transform.SetAsFirstSibling();//change render order textcolor->text

            //Change Color of Textbackground
            var nColor = Color.white;
            switch(setting.DifficaltyType)
            {
                case 0:
                    nColor = new Color(0,0,0,0);
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

                case 5:
                    nColor = new Color(setting.DifficaltyR, setting.DifficaltyG, setting.DifficaltyB, 1.0f);
                    break;
            }
            textcolor.GetComponent<Image>().color = nColor;

            FullScreenCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        }

        public void EditLanotaTheme(LanotaThemeSetting setting)
        {
            //Ready for Init, change Fullscreen canvas mode
            Header.GetComponent<Image>().overrideSprite = TopBarSprite;
            FullScreenCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

            //Setting Text
            if (setting.LanotaFont)
            {
                Copyright.GetComponent<Text>().font = Parabola;
                ChartName.GetComponent<Text>().font = Kawoszeh;
                ChartDesigner_Placeholder.GetComponent<Text>().font = Kawoszeh;
                ChartDesigner_Text.GetComponent<Text>().font = Kawoszeh;
            }
            else
            {
                Copyright.GetComponent<Text>().font = DefaultFont;
                ChartName.GetComponent<Text>().font = DefaultFont;
                ChartDesigner_Placeholder.GetComponent<Text>().font = DefaultFont;
                ChartDesigner_Text.GetComponent<Text>().font = DefaultFont;
            }
            Copyright.GetComponent<Text>().text = setting.Difficalty;

            //Create Prefab Object
            var pausebtn = GameObject.Find("PauseButton");
            var textcolor = GameObject.Find("DiffTextColor");

            textcolor.transform.SetAsFirstSibling();//change render order textcolor->text

            //Change Color of Textbackground
            var nColor = Color.white;
            switch (setting.DifficaltyType)
            {
                case 0:
                    nColor = new Color(0, 0, 0, 0);
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

                case 5:
                    nColor = new Color(setting.DifficaltyR, setting.DifficaltyG, setting.DifficaltyB, 1.0f);
                    break;
            }
            textcolor.GetComponent<Image>().color = nColor;

            FullScreenCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        }

        public bool IsLanotaThemeLoaded()
        {
            return GameObject.Find("PauseButton") != null;
        }

        public string Name(Language language)
        {
            return "Customize Lanotalium Ui";
        }

        public string Description(Language language)
        {
            return "Header Customize, Tuner Alpha, Lanota Theme";
        }
    }
}
