using Lanotalium.Plugin;
using System.Collections.Generic;
using UnityEngine;

namespace Flowaria.AutorunPlugin
{
    public class SingleShotObject : MonoBehaviour
    {
        public List<LanotaliumAutorun> Plugins;
        public string PluginPath;

        private void LateUpdate()
        {
            var context = new LanotaliumContext()
            {
                IsProjectLoaded = (LimProjectManager.CurrentProject != null && LimTunerManager.Instance.isInitialized),
                CurrentProject = LimProjectManager.CurrentProject,
                CurrentLanguage = LimLanguageManager.CurrentLanguage == "简体中文" ? Language.简体中文 : Language.English,
                EditorManager = LimEditorManager.Instance,
                TunerManager = LimTunerManager.Instance,
                OperationManager = LimOperationManager.Instance,
                UserRequest = EasyRequest.EasyRequestManager.Instance,
                MessageBox = MessageBoxManager.Instance
            };

            foreach (var plugin in Plugins)
            {
                var configPath = PathUtil.GetConfigPath(plugin.Name);
                StartCoroutine(plugin.Method(configPath, context));
            }
            this.enabled = false;
        }
    }
}