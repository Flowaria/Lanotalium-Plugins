using Lanotalium.Plugin;
using Lanotalium.Plugin.Simple.RTRequest;
using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_time_Request_Test
{
    public class Class1 : ILanotaliumPlugin
    {
        public string Description(Language language)
        {
            return "test";
        }

        public string Name(Language language)
        {
            return "Test";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            var rtRequest = new RuntimeRequest(context.UserRequest);
            rtRequest.CreateField("ValueOne", "Int Value : 1", RequestFieldType.Int);
            rtRequest.CreateField("ValueTwo", "Int Value : 2", RequestFieldType.Int).SetRange(-5, 5);
            rtRequest.CreateField("ValueThree", "Float Value : 3", RequestFieldType.Float).SetRange(-10,10).SetDefaultValue(5.0f);
            rtRequest.CreateField("ValueFour", "Bool Value : 4", RequestFieldType.Bool);
            rtRequest.CreateField("ValueFive", "String Value : 5", RequestFieldType.String);
            yield return rtRequest.Request();

            if(rtRequest.Result.Success)
            {
                context.MessageBox.ShowMessage("Success!");

                int i1 = (int)rtRequest.Result.ReadField("ValueOne");
                int i2 = (int)rtRequest.Result.ReadField("ValueTwo");
                float f1 = (float)rtRequest.Result.ReadField("ValueThree");
                bool b1 = (bool)rtRequest.Result.ReadField("ValueFour");
                string s1 = (string)rtRequest.Result.ReadField("ValueFive");

                context.MessageBox.ShowMessage(string.Join("\n", i1, i2, f1, b1, s1));
            }
            else
            {
                context.MessageBox.ShowMessage("Failed!");
            }
        }
    }
}
