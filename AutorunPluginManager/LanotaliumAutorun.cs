using Lanotalium.Plugin;
using System;
using System.Collections;

namespace Flowaria.AutorunPlugin
{
    using AutorunCallback = Func<string, LanotaliumContext, IEnumerator>;

    public class LanotaliumAutorun
    {
        public AutorunCallback Method;
        public string Name;
    }
}