using EasyRequest;
using Lanotalium.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Flowaria.Lanotalium.Plugin.Tweak
{
    public class AskForSkin
    {
        [Name("What is the Name of the tuner skin?")]
        public string Name = "Default";

        [Name("Alpha of the Tuner Background")]
        [Range(0.0f, 1.0f)]
        public float Alpha = 0.6f;

        [Name("Do you want to associate this skin with this project? (Not working now)")]
        public bool Associate = false;
    }

    public class TunerSkinTweak : ILanotaliumPlugin
    {
        public string Name(Language language)
        {
            return "UiTweak - Tuner Skin (Beta v2)";
        }

        public string Description(Language language)
        {
            return "Change the image of tuner";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            if (!context.IsProjectLoaded)
            {
                context.MessageBox.ShowMessage("You must load the project first");
            }
            else
            {
                Request<AskForSkin> request = new Request<AskForSkin>();
                yield return context.UserRequest.Request(request, "Tuner Setting");
                if (request.Succeed)
                {
                    var r = request.Object;
                    var directory = Path.Combine(Application.streamingAssetsPath, String.Format("TunerSkin/{0}", r.Name));
                    TunerSkin skin;
                    if ((skin = TunerSkin.LoadFromDirectory(directory)) != null)
                    {
                        var Background = GameObject.Find("Tuner/Background");
                        var Border = GameObject.Find("Tuner/Border");
                        var JudgeLine = GameObject.Find("Tuner/JudgeLine");
                        var Arrow = GameObject.Find("Tuner/Arrow");
                        var Core = GameObject.Find("Tuner/Core");

                        Background.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, r.Alpha);
                        Background.GetComponent<SpriteRenderer>().sprite = skin.DefaultSprite.Backgroud;
                        Border.GetComponent<SpriteRenderer>().sprite = skin.DefaultSprite.Border;
                        Core.GetComponent<SpriteRenderer>().sprite = skin.DefaultSprite.Core;
                        Arrow.GetComponent<SpriteRenderer>().sprite = skin.DefaultSprite.Arrow;
                        JudgeLine.GetComponent<SpriteRenderer>().sprite = skin.DefaultSprite.Judgeline;
                    }
                }

            }
            yield return null;
        }
    }

    public class TunerSprite
    {
        public Sprite Arrow { get; private set; }
        public Sprite Backgroud { get; private set; }
        public Sprite Border { get; private set; }
        public Sprite Core { get; private set; }
        public Sprite Judgeline { get; private set; }

        private TunerSprite() { }

        public static TunerSprite LoadFromDirectory(string directory)
        {
            var sprite = new TunerSprite();
            sprite.Arrow = LoadSprite(Path.GetFullPath(Path.Combine(directory, "Arrow.png")));
            sprite.Backgroud = LoadSprite(Path.GetFullPath(Path.Combine(directory, "Background.png")));
            sprite.Border = LoadSprite(Path.GetFullPath(Path.Combine(directory, "Border.png")));
            sprite.Core = LoadSprite(Path.GetFullPath(Path.Combine(directory, "Core.png")));
            sprite.Judgeline = LoadSprite(Path.GetFullPath(Path.Combine(directory, "Judgeline.png")));
            return sprite;
        }

        public static Sprite LoadSprite(string absoluteImagePath)
        {
            string finalPath;
            WWW localFile;
            Texture texture;

            finalPath = "file://" + absoluteImagePath;
            localFile = new WWW(finalPath);

            texture = localFile.texture;
            return Sprite.Create(texture as Texture2D, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
    public class TunerSkin
    {
        public bool UseTunerMode = false;
        public string Name = "Skin Name";
        public TunerSprite DefaultSprite, PurifySprite;

        private TunerSkin() { }

        public static TunerSkin LoadFromDirectory(string directory)
        {
            var skin =  new TunerSkin();
            skin.DefaultSprite = TunerSprite.LoadFromDirectory(directory);
            return skin;
        }
    }
}
