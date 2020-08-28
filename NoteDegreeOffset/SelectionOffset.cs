using EasyRequest;
using Lanotalium.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteDegreeOffset
{
    public class SelectionOffset : ILanotaliumPlugin
    {
        public string Name(Language language)
        {
            return "Degree Offset (From Selection)";
        }

        public string Description(Language language)
        {
            return "Add Offset to Degree to Selected Notes";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            if (!context.IsProjectLoaded)
            {
                yield break;
            }

            var request = new Request<AskOffset>();
            yield return context.UserRequest.Request(request, "Selection Based Degree Offset");

            if (!request.Succeed)
            {
                yield break;
            }

            var result = request.Object;
            var operationManager = context.OperationManager;

            foreach (var tapNote in operationManager.SelectedTapNote)
            {
                var newDegree = tapNote.Degree + result.Degree;
                operationManager.SetTapNoteDegree(tapNote, newDegree, false);
            }

            foreach (var holdNote in operationManager.SelectedHoldNote)
            {
                var newDegree = holdNote.Degree + result.Degree;
                operationManager.SetHoldNoteDegree(holdNote, newDegree, false);
            }
        }
    }

    public class AskOffset
    {
        [Name("Offset Degree")]
        public float Degree = 0.0f;
    }
}
