using BepInEx;
using Lanotalium.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Flowaria.AutorunPlugin
{
    using AutorunCallback = Func<string, LanotaliumContext, IEnumerator>;

    [BepInPlugin("flowaria.bepinex.autorunplugin", "Autorun Plug-in", "1.0.0.0")]
    [BepInProcess("Lanotalium.exe")]
    public class AutorunPluginEntry : BaseUnityPlugin
    {
        internal string PluginPath;
        internal List<LanotaliumAutorun> Plugins = new List<LanotaliumAutorun>();

        private void Awake()
        {
            PluginPath = PathUtil.GetPluginPath();

            var configBasePath = PluginPath + "/Config";
            if (!Directory.Exists(configBasePath))
            {
                Directory.CreateDirectory(configBasePath);
            }

            InitPlugins();
            SceneManager.activeSceneChanged += SceneChanged;
        }

        private void InitPlugins()
        {
            string[] files = Directory.GetFiles(PluginPath);

            foreach (string file in files)
            {
                if (!file.EndsWith(".dll"))
                    continue;

                try
                {
                    Assembly a = Assembly.LoadFile(file);
                    foreach (Type t in a.GetTypes())
                    {
                        Type i = t.GetInterface("ILanotaliumAutorun");

                        //when it don't have interface
                        //Found every method that meeting condition
                        if (i == null)
                        {
                            foreach (var method in t.GetMethods())
                            {
                                //check return type
                                if (method.ReturnType == null)
                                    continue;
                                if (method.ReturnType != typeof(IEnumerator))
                                    continue;

                                //check parameters
                                var parameters = method.GetParameters();
                                if (parameters == null)
                                    continue;
                                if (parameters.Length != 2)
                                    continue;
                                if (parameters[0].ParameterType != typeof(string))
                                    continue;
                                if (parameters[1].ParameterType != typeof(LanotaliumContext))
                                    continue;

                                //check attributes
                                var iNameAttr = method.GetCustomAttribute<PluginNameAttribute>();
                                if (iNameAttr == null)
                                    continue;

                                //MethodInfo to Func
                                Delegate d;
                                if (method.IsStatic)
                                {
                                    d = method.CreateDelegate(typeof(AutorunCallback));
                                }
                                else
                                {
                                    var o = Activator.CreateInstance(t);
                                    d = method.CreateDelegate(typeof(AutorunCallback), o);
                                }

                                var f = (AutorunCallback)d;
                                AddPlugin(iNameAttr.Name, f);
                            }
                        }
                        //when it does have ILanotaliumAutorun
                        //... Honestly just use this method
                        else
                        {
                            var nameAttr = t.GetCustomAttribute<PluginNameAttribute>();
                            if (nameAttr == null)
                                continue;

                            var interfac = Activator.CreateInstance(t) as ILanotaliumAutorun;

                            AddPlugin(nameAttr.Name, interfac.OnLoaded);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Log($"ERROR: {e.Message}");
                    continue;
                }
            }
        }

        private readonly Regex _NameRegex = new Regex(@"^[\w\-. ]+$");

        private void AddPlugin(string name, AutorunCallback func)
        {
            if (string.IsNullOrEmpty(name))
                return;

            if (!_NameRegex.IsMatch(name))
                return;

            if (Plugins.Any(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                return;

            var plugin = new LanotaliumAutorun()
            {
                Method = func,
                Name = name
            };

            Plugins.Add(plugin);
            Debug.Log($"ADDED: {plugin.Name}, {plugin.Method.Method.Name}");
        }

        private void SceneChanged(Scene current, Scene next)
        {
            if (next.name.Equals("LimTuner"))
            {
                var obj = new GameObject();

                var param = obj.AddComponent<SingleShotObject>();
                param.PluginPath = PluginPath;
                param.Plugins = Plugins;
            }
        }
    }
}