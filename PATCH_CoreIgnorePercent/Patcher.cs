using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATCH_CoreIgnorePercent
{
    public class Patcher
    {
        public static IEnumerable<string> TargetDLLs { get; } = new[] { "Schwarzer.Lanotalium.dll" };

        public static void Patch(AssemblyDefinition assembly)
        {
            PatchPercent(assembly, 20.0f, 10.0f, "LimHoldNoteManager",
                "UpdateAllJointActive",
                "UpdateAllLineMaterial",
                "UpdateAllLineRenderers",
                "UpdateAllNoteActive",
                "UpdateAllNoteTransforms",
                "UpdateJointTransform");
            PatchPercent(assembly, 20.0f, 10.0f, "LimTapNoteManager",
               "UpdateAllNoteActive",
               "UpdateAllNoteTransforms");
            PatchPercent(assembly, 20.0f, 10.0f, "ComponentBpmManager", "UpdateBeatline");

            //

            //17.5
            float speedConst = 14.5f;
            PatchPercent(assembly, 10.0f, speedConst, "LimHoldNoteManager", "CalculateMovePercent");
            PatchPercent(assembly, 10.0f, speedConst, "LimTapNoteManager", "CalculateMovePercent");
            PatchPercent(assembly, 10.0f, speedConst, "LimCameraManager", "CalculateMovePercent");
            PatchPercent(assembly, 10.0f, speedConst, "ComponentBpmManager", "CalculateMovePercent");
            PatchPercent(assembly, 10.0f, speedConst, "LimClickToCreateManager", "CalculateMovePercent", "CalculateCurserTime");

            //

            PatchPercent(assembly, 75.5f, 72f, "LimScanTime",
                "TryCalculateKeyTimes");

            PatchPercent(assembly, 0.2f, 0.1f, "LimScanTime",
               "IsTapNoteinScanRange");
        }

        public static void PatchPercent(AssemblyDefinition assembly, float currentValue, float changeValue, string classname, params string[] methods)
        {
            var _type = assembly.MainModule.GetType(classname);
            if (_type == null)
            {
                return;
            }

            foreach(var methodname in methods)
            {
                var method = _type.Methods.FirstOrDefault(x => x.Name.Equals(methodname));
                if (method == default(MethodDefinition))
                {
                    continue;
                }

                if (!method.HasBody)
                {
                    continue;
                }

                Console.WriteLine("checking method {0}:{1}", classname, methodname);

                foreach (var inst in method.Body.Instructions)
                {
                    if (inst.OpCode == OpCodes.Ldc_R4)
                    {
                        if ((float)inst.Operand != currentValue)
                        {
                            continue;
                        }

                        Console.WriteLine("---matching found!");

                        inst.Operand = changeValue;
                    }
                    else if (inst.OpCode == OpCodes.Ldc_R8)
                    {
                        if ((float)((double)inst.Operand) != currentValue)
                        {
                            continue;
                        }

                        Console.WriteLine("---matching found!");

                        inst.Operand = (double)changeValue;
                    }

                    
                }
            }
        }
    }
}
