using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyRequest;
using UnityEngine;

namespace Flowaria.Lanotalium.Plugin
{
    public class Setting
    {
        [Name("Alpha of the Tuner Background")]
        [Range(0.0f, 1.0f)]
        public float Alpha = 0.6f;

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

    public class LanotaThemeSetting
    {
        [Name("Alpha of the Header")]
        [Range(0.0f, 1.0f)]
        public float HeaderAlpha = 1.0f;

        [Name("Difficulty")]
        public string Difficalty = "Master 15";

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

        [Name("Use Lanota Font for Header")]
        public bool LanotaFont = true;
    }
}
