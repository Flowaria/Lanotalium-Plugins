using Lanotalium.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSAPI.Chart
{
    public partial class TapNote
    {
        public LanotaTapNote Note;
        public TapNote Copy()
        {
            return Note.DeepCopy();
        }
    }

    public partial class TapNote
    {

    }
}
