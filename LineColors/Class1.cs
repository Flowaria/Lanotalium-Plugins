using EasyRequest;
using Lanotalium.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LineColors
{
    public class LinestyleContext
    {
        [Name("Line Width (default: 0.1)")]
        [Range(0.0f, 1.0f)]
        public float Width = 0.1f;

        [Name("Color R")]
        [Range(0.0f, 1.0f)]
        public float R = 1.0f;

        [Name("Color G")]
        [Range(0.0f, 1.0f)]
        public float G = 1.0f;

        [Name("Color B")]
        [Range(0.0f, 1.0f)]
        public float B = 1.0f;
    }

    public class AngleLineColors : ILanotaliumPlugin
    {
        public string Description(Language language)
        {
            return "Change Line";
        }

        public string Name(Language language)
        {
            return "Change Angleline Style™";
        }

        public Material lineMaterial;
        public IEnumerator Process(LanotaliumContext context)
        {
            Request<LinestyleContext> r = new Request<LinestyleContext>();

            yield return context.UserRequest.Request(r, "Angleline Style");
            if(r.Succeed)
            {
                var AnglelinePrefab = context.EditorManager.CreatorWindow.AngleLineManager.AnglelinePrefab;
                var AnglelineMat = AnglelinePrefab.GetComponent<LineRenderer>().material;


                var o = r.Object;
                Util.ApplyLineWidth(AnglelinePrefab, "AnglelinePrefab(Clone)", o.Width);
                Util.ApplyColorToMaterial(AnglelineMat, new Color(o.R, o.G, o.B, 1.0f), "AnglelinePrefab(Clone)");
            }
        }
    }

    public class BeatLineColors : ILanotaliumPlugin
    {
        public string Description(Language language)
        {
            return "Change Line";
        }

        public string Name(Language language)
        {
            return "Change Beatline Style™";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            

            Request<LinestyleContext> r = new Request<LinestyleContext>();

            yield return context.UserRequest.Request(r, "Beatline Style");
            if (r.Succeed)
            {
                var BeatlinePrefab = context.EditorManager.InspectorWindow.ComponentBpm.BeatlinePrefab;
                var BeatlineMat = BeatlinePrefab.GetComponent<LineRenderer>().material;

                var o = r.Object;
                Util.ApplyLineWidth(BeatlinePrefab, "BeatlinePrefab(Clone)", o.Width);
                Util.ApplyColorToMaterial(BeatlineMat, new Color(o.R, o.G, o.B, 1.0f), "BeatlinePrefab(Clone)");
                
            }
        }
    }
    
    public static class Util
    {
        public static void ApplyColorToMaterial(Material mat, Color c, string obj_name)
        {
            mat.color = c;


            foreach (var o in Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == obj_name))
            {
                o.GetComponent<LineRenderer>().material.color = c;
            }
        }

        public static void ApplyLineWidth(GameObject preset, string obj_name, float width)
        {
            preset.GetComponent<LineRenderer>().widthMultiplier = width;

            foreach (var o in Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == obj_name))
            {
                o.GetComponent<LineRenderer>().widthMultiplier = width;
            }
        }
    }
}
