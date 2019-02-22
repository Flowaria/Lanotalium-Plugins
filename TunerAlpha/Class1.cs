using EasyRequest;
using Lanotalium.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TunerAlpha
{
    public class AlphaContext
    {
        [Name("Background Alpha (default: 0.6)")]
        [Range(0.0f, 1.0f)]
        public float aBG = TunerAlpha.AlphaBackground.a;

        [Name("Border Alpha (default: 1.0)")]
        [Range(0.0f, 1.0f)]
        public float aBorder = TunerAlpha.AlphaBorder.a;

        [Name("Arrow Alpha (default: 1.0)")]
        [Range(0.0f, 1.0f)]
        public float aArrow = TunerAlpha.AlphaArrow.a;

        [Name("Core Alpha (default: 1.0)")]
        [Range(0.0f, 1.0f)]
        public float aCore = TunerAlpha.AlphaCore.a;

        [Name("JudgeLine Alpha (default: 1.0)")]
        [Range(0.0f, 1.0f)]
        public float aJudge = TunerAlpha.AlphaJudgeLine.a;
    }

    public class TunerAlpha : ILanotaliumPlugin
    {
        public static Color AlphaBackground
        {
            get
            {
                var obj = GameObject.Find("Tuner/Background");
                if (obj != null)
                {
                    return obj.GetComponent<SpriteRenderer>().color;
                }
                return Color.white;
            }
            set
            {
                var obj = GameObject.Find("Tuner/Background");
                if (obj != null)
                {
                    obj.GetComponent<SpriteRenderer>().color = value;
                }
            }
        }

        public static Color AlphaBorder
        {
            get
            {
                var obj = GameObject.Find("Tuner/Border");
                if (obj != null)
                {
                    return obj.GetComponent<SpriteRenderer>().color;
                }
                return Color.white;
            }
            set
            {
                var obj = GameObject.Find("Tuner/Border");
                if (obj != null)
                {
                    obj.GetComponent<SpriteRenderer>().color = value;
                }
            }
        }

        public static Color AlphaArrow
        {
            get
            {
                var obj = GameObject.Find("Tuner/Arrow");
                if (obj != null)
                {
                    return obj.GetComponent<SpriteRenderer>().color;
                }
                return Color.white;
            }
            set
            {
                var obj = GameObject.Find("Tuner/Arrow");
                if (obj != null)
                {
                    obj.GetComponent<SpriteRenderer>().color = value;
                }
            }
        }

        public static Color AlphaCore
        {
            get
            {
                var obj = GameObject.Find("Tuner/Core");
                if (obj != null)
                {
                    return obj.GetComponent<SpriteRenderer>().color;
                }
                return Color.white;
            }
            set
            {
                var obj = GameObject.Find("Tuner/Core");
                if (obj != null)
                {
                    obj.GetComponent<SpriteRenderer>().color = value;
                }
            }
        }

        public static Color AlphaJudgeLine
        {
            get
            {
                var obj = GameObject.Find("Tuner/JudgeLine");
                if (obj != null)
                {
                    return obj.GetComponent<SpriteRenderer>().color;
                }
                return Color.white;
            }
            set
            {
                var obj = GameObject.Find("Tuner/JudgeLine");
                if (obj != null)
                {
                    obj.GetComponent<SpriteRenderer>().color = value;
                }
            }
        }

        public string Description(Language language)
        {
            return "description";
        }

        public string Name(Language language)
        {
            return "Tuner Alpha";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            Request<AlphaContext> r = new Request<AlphaContext>();

            yield return context.UserRequest.Request(r, "Alpha Master");

            if (r.Succeed)
            {
                var o = r.Object;
                AlphaBackground = new Color(1.0f,1.0f,1.0f,o.aBG);
                AlphaBorder = new Color(1.0f, 1.0f, 1.0f, o.aBorder);
                AlphaArrow = new Color(1.0f, 1.0f, 1.0f, o.aArrow);
                AlphaCore = new Color(1.0f, 1.0f, 1.0f, o.aCore);
                AlphaJudgeLine = new Color(1.0f, 1.0f, 1.0f, o.aJudge);
            }
        }
    }
}
