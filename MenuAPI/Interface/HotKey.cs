using Lanotalium.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShortCutAPI.Interface
{

    public interface IHotKey
    {
        KeyCode Key1();
        KeyCode Key2();
        KeyCode Key3();
        KeyCode Key4();
        IEnumerator OnPressed(LanotaliumContext context);
    }

    public class InterfaceHotKeyManager
    {

    }
}
