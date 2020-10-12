using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PATCH_OffLimitLanotalium
{
    public class Patcher
    {
        public static IEnumerable<string> TargetDLLs { get; } = new[] { "Schwarzer.Lanotalium.dll" };

        public static void Patch(AssemblyDefinition assembly)
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configFile = Path.Combine(basePath, "offlimit.config.txt");

            Console.WriteLine(configFile);

            if(!File.Exists(configFile))
            {
                return;
            }

            float beatline_maxcount = 0;
            int angleline_maxcount = 0;
            foreach (var line in File.ReadAllLines(configFile))
            {
                int value_i = 0;
                float value_f = 0.0f;
                if(TryReadIntConfig(line, "angleline_maxcount=", out value_i))
                {
                    if(value_i > 0)
                    {
                        angleline_maxcount = value_i;
                    }
                }
                if (TryReadFloatConfig(line, "beatline_maxcount=", out value_f))
                {
                    if(value_f > 0.0f)
                    {
                        beatline_maxcount = value_f;
                    }
                }
            }

            bool TryReadIntConfig(string line, string entry, out int value)
            {
                if (line.StartsWith(entry))
                {
                    if(int.TryParse(line.Replace(entry, ""), out value))
                    {
                        return true;
                    }
                }

                value = -1;
                return false;
            }

            bool TryReadFloatConfig(string line, string entry, out float value)
            {
                if (line.StartsWith(entry))
                {
                    if (float.TryParse(line.Replace(entry, ""), out value))
                    {
                        return true;
                    }
                }

                value = -1.0f;
                return false;
            }

            if(angleline_maxcount > 0)
            {
                PatchInt(assembly, 24, angleline_maxcount, "LimAngleLineManager", "TryParseRange");
            }
            if(beatline_maxcount > 0.0f)
            {
                PatchFloat(assembly, 16.0f, beatline_maxcount, "ComponentBpmManager", "OnDensityChange");
            }
        }

        public static void PatchFloat(AssemblyDefinition assembly, float currentValue, float changeValue, string classname, params string[] methods)
        {
            var _type = assembly.MainModule.GetType(classname);
            if (_type == null)
            {
                return;
            }

            foreach (var methodname in methods)
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

        public static void PatchInt(AssemblyDefinition assembly, int currentValue, int changeValue, string classname, params string[] methods)
        {
            var _type = assembly.MainModule.GetType(classname);
            if (_type == null)
            {
                return;
            }

            foreach (var methodname in methods)
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
                    if(!IsIntOpCode(inst.OpCode))
                    {
                        continue;
                    }

                    if (GetIntValue(inst) == currentValue)
                    {
                        Console.WriteLine("---matching found!");

                        SetIntValue(inst, changeValue);
                    }
                }
            }
        }

        public static bool IsIntOpCode(OpCode code)
        {
            if (code == OpCodes.Ldc_I4 ||
                code == OpCodes.Ldc_I4_0 ||
                code == OpCodes.Ldc_I4_1 ||
                code == OpCodes.Ldc_I4_2 ||
                code == OpCodes.Ldc_I4_3 ||
                code == OpCodes.Ldc_I4_4 ||
                code == OpCodes.Ldc_I4_5 ||
                code == OpCodes.Ldc_I4_6 ||
                code == OpCodes.Ldc_I4_7 ||
                code == OpCodes.Ldc_I4_8 ||
                code == OpCodes.Ldc_I4_M1 ||
                code == OpCodes.Ldc_I4_S ||
                code == OpCodes.Ldc_I8)
            {
                return true;
            }
            return false;
        }

        public static void SetIntValue(Instruction inst, int value)
        {
            if(IsIntOpCode(inst.OpCode))
            {
                OpCode newOpCode = OpCodes.Nop;
                switch(value)
                {
                    case -1:    newOpCode = OpCodes.Ldc_I4_M1; break;
                    case 0:     newOpCode = OpCodes.Ldc_I4_0; break;
                    case 1:     newOpCode = OpCodes.Ldc_I4_1; break;
                    case 2:     newOpCode = OpCodes.Ldc_I4_2; break;
                    case 3:     newOpCode = OpCodes.Ldc_I4_3; break;
                    case 4:     newOpCode = OpCodes.Ldc_I4_4; break;
                    case 5:     newOpCode = OpCodes.Ldc_I4_5; break;
                    case 6:     newOpCode = OpCodes.Ldc_I4_6; break;
                    case 7:     newOpCode = OpCodes.Ldc_I4_7; break;
                    case 8:     newOpCode = OpCodes.Ldc_I4_8; break;
                    default:    newOpCode = OpCodes.Ldc_I4; break;
                }

                if (newOpCode == OpCodes.Ldc_I4)
                {
                    if(sbyte.MinValue <= value && value <= sbyte.MaxValue)
                    {
                        newOpCode = OpCodes.Ldc_I4_S;
                    }
                }

                inst.OpCode = newOpCode;
                if(newOpCode == OpCodes.Ldc_I4_S)
                {
                    inst.Operand = (sbyte)value;
                }
                else
                {
                    inst.Operand = value;
                }
            }
        }

        public static int GetIntValue(Instruction inst)
        {
            switch (inst.OpCode.Code)
            {
                case Code.Ldc_I4_0: return 0;
                case Code.Ldc_I4_1: return 1;
                case Code.Ldc_I4_2: return 2;
                case Code.Ldc_I4_3: return 3;
                case Code.Ldc_I4_4: return 4;
                case Code.Ldc_I4_5: return 5;
                case Code.Ldc_I4_6: return 6;
                case Code.Ldc_I4_7: return 7;
                case Code.Ldc_I4_8: return 8;
                case Code.Ldc_I4_S:
                    return (sbyte)inst.Operand;
                case Code.Ldc_I4: return (int)inst.Operand;

                case Code.Ldc_I4_M1:
                default:
                    return -1;
            }
        }
    }
}
