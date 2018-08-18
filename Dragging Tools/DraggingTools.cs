using Lanotalium.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowaria.Lanotalium.Plugin
{
    public class DraggingTools : ILanotaliumPlugin
    {
        public string Name(Language language)
        {
            return "Dragging Tool";
        }

        public string Description(Language language)
        {
            return "I want to drag my notes/motions :`<";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            context.MessageBox.ShowMessage("[Dragging Tool] Plugin has been Started!\n'Shift+Left Mouse' to Drag Stuff in Your Cursor!\nIf you need Snap Press 's' and it will shows menu\nIf you select end of the motion, you can change the duration of the motion!\nTo Toggle the feature, Press 'F8' and if you want to exit, Press with 'Shift'");

            yield return null;
        }
    }
}
