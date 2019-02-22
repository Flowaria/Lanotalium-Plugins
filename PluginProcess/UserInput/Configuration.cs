using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lanotalium.Plugin.Simple.UserInput
{
    public class Configuration
    {
        public static T LoadConfig<T>(string file)
        {
            if(File.Exists(file))
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(file));
            }
            return default(T);
        }

        public static bool SaveConfig<T>(string file, T cfg)
        {
            try
            {
                File.WriteAllText(file, JsonConvert.SerializeObject(cfg));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
