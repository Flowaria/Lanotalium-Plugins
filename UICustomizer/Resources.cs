using Lanotalium.Plugin.Simple.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Flowaria.Lanotalium.Plugin.Tweak
{
    public class Resources
    {
        public class ParticleResources
        {
            [ResourceName("SparkleFlickOut")]
            public GameObject Prefab_SparkleFlickOut;

            [ResourceName("SparkleFlickIn")]
            public GameObject Prefab_SparkleFlickIn;

            [ResourceName("SparkleHold")]
            public GameObject Prefab_SparkleHold;

            [ResourceName("ShockwaveClick")]
            public GameObject Prefab_ShockwaveClick;

            [ResourceName("ShockwaveHoldStart")]
            public GameObject Prefab_ShockwaveHoldStart;

            [ResourceName("ShockwaveHoldMiddle")]
            public GameObject Prefab_ShockwaveHoldMiddle;

            [ResourceName("ShockwaveHoldEnd")]
            public GameObject Prefab_ShockwaveHoldEnd;

            [ResourceName("harmonyeffect")]
            public GameObject Prefab_harmonyeffect;

            [ResourceName("ComboText")]
            public GameObject Prefab_ComboTextCluster;

            [ResourceName("Assets/UiTweak/ParticleTweak/Fonts/kawoszeh.ttf")]
            public Font Font_Kawoszeh;
        }

        public class OrnamentResources
        {
            [ResourceName("JudgeLineGlow")]
            public GameObject Prefab_JudgeGlow;

            [ResourceName("JudgeLineOrnament")]
            public GameObject Prefab_JudgeOrnament;
        }

        public class LanotaHeaderResources
        {
            [ResourceName("LanotaHeader")]
            public GameObject Prefab_LanotaHeader;

            [ResourceName("TopBar")]
            public Sprite Sprite_TopBar;

            [ResourceName("Assets/UiTweak/HeaderTweak/Fonts/kawoszeh_header.ttf")]
            public Font Font_Kawoszeh;

            [ResourceName("Assets/UiTweak/HeaderTweak/Fonts/parabola_header.ttf")]
            public Font Font_Parabola;
        }
    }
}
