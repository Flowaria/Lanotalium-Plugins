using Lanotalium.Plugin;
using System.Collections;

namespace Flowaria.AutorunPlugin
{
    public interface ILanotaliumAutorun
    {
        IEnumerator OnLoaded(string configPath, LanotaliumContext context);
    }
}