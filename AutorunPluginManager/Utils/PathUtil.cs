using UnityEngine;

namespace Flowaria.AutorunPlugin
{
    public static class PathUtil
    {
        public static string GetPluginPath()
        {
            return Application.streamingAssetsPath + "/Plugins";
        }

        public static string GetConfigPath(string pluginName)
        {
            return GetPluginPath() + $"/Config/{pluginName.ToLower()}.json";
        }
    }
}