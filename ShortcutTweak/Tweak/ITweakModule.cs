using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowaria.Lanotalium.Plugin.Tweak
{
    public interface ITweakModule
    {
        string GetModuleName();
        void Enable();
        void Disable();
    }
}
