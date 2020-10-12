using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Flowaria.Railnote.Curve.Patch
{
    [HarmonyPatch(typeof(LimHoldNoteManager))]
    [HarmonyPatch("UpdateAllLineRenderers")]
    class HoldNoteManager_UpdateAllLineRenderers
    {
        static bool Prefix()
        {
            return false;
        }
    }
}
