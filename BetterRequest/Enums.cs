using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lanotalium.Plugin.BetterRequest
{
    public enum RequestType
    {
        TextField,
        NumberField,
        Slider,
        CheckBox,
        RadioBox,
        ListBox,
        DropdownListBox,
        ColorPicker,
        FileChooser
    }

    public enum RequestFlag
    {
        NoAlphaPicker,
        CheckBox
    }
}
