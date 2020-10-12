using Mono.Cecil;
using Mono.Cecil.Cil;
using PATCH_LanotaCurvedRail.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PATCH_LanotaCurvedRail
{
    public class Patcher
    {
        public static IEnumerable<string> TargetDLLs { get; } = new[] { "Schwarzer.Lanotalium.dll" };

        public static void Patch(AssemblyDefinition assembly)
        {
            /*
            

            foreach(var module in currentAssembly.MainModule.Types)
            {
                Console.WriteLine(module.Name);
            }
            var currentAssembly = AssemblyDefinition.ReadAssembly(Assembly.GetExecutingAssembly().Location);
            var module = currentAssembly.Modules.First(x=>x.FileName.Equals("PATCH_LanotaCurvedRail.Content"));

            //var reference = assembly.MainModule.ImportReference(typeof(HoldNoteLineRenderer));

            //assembly.MainModule.Types.Add(currentAssembly.MainModule.Types.First(x => x.Name.Equals("HoldNoteLineRenderer")));
            */

            //assembly.MainModule.ImportReference(typeof(HoldNoteLineRenderer));
            //var refer = assembly.MainModule.ImportReference(typeof(HoldNoteManagerPatch));
            //assembly.MainModule.ImportReference(typeof(LimTunerManager));

            var holdManager = assembly.MainModule.Types.FirstOrDefault(x=>x.Name.Equals("LimHoldNoteManager"));
            if (holdManager == null)
            {
                return;
            }

            var method = holdManager.Methods.FirstOrDefault(x=>x.Name.Equals("UpdateAllLineRenderers"));
            if(method == null)
            {
                return;
            }

            //method.Body.Instructions.Clear();

            var refer = assembly.MainModule.ImportReference(typeof(HoldNoteManagerPatch).GetMethod("UpdateAllLineRenderers_new"));

            //MethodReference writeline = assembly.MainModule.Import(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
            method.Body.Instructions.Clear();
            //method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Call, refer));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));

            assembly.Write("test.dll");
        }
    }
}
