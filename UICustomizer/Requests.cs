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
        [Default(0.6f)]
        public float Alpha;

        [Name("Show Default Header (when disable, text will be transparent to)")]
        [Default(true)]
        public bool DefaultHeader;

        [Name("└R")]
        [Range(0.0f, 1.0f)]
        [Default(0.392f)]
        public float DefaultHeaderR;

        [Name("└G")]
        [Range(0.0f, 1.0f)]
        [Default(0.392f)]
        public float DefaultHeaderG;

        [Name("└B")]
        [Range(0.0f, 1.0f)]
        [Default(0.392f)]
        public float DefaultHeaderB;

        [Name("└Alpha of the Header")]
        [Range(0.0f, 1.0f)]
        [Default(1.0f)]
        public float DefaultHeaderAlpha;

        [Name("Use Official Lanota Style Header (this cannot be rolled-back until you load other project)")]
        [Default(false)]
        public bool LanotaHeader;
    }

    public class LanotaThemeSetting
    {
        [Name("Alpha of the Header")]
        [Range(0.0f, 1.0f)]
        [Default(1.0f)]
        public float HeaderAlpha;

        [Name("Difficulty")]
        [Default("Master 15")]
        public string Difficalty;

        [Name("Difficulty Color Type\n0: invisible, 1: whisper, 2: acoustic, 3: ultra, 4: master, 5: custom")]
        [Range(0, 5)]
        [Default(4)]
        public int DifficaltyType;

        [Name("└Custom Color R")]
        [Range(0.0f, 1.0f)]
        [Default(1.0f)]
        public float DifficaltyR;

        [Name("└Custom Color G")]
        [Range(0.0f, 1.0f)]
        [Default(1.0f)]
        public float DifficaltyG;

        [Name("└Custom Color B")]
        [Range(0.0f, 1.0f)]
        [Default(1.0f)]
        public float DifficaltyB;

        [Name("Use Lanota Font for Header")]
        [Default(true)]
        public bool LanotaFont;
    }
}
