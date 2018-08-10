using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EasyRequest;
using Lanotalium.Chart;
using Lanotalium.Plugin;
using ShortCutAPI.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace ShortCutAPI
{
    public class Main : ILanotaliumPlugin
    {
        public string Name(Language language)
        {
            return "Shortcut Bridge API";
        }

        public string Description(Language language)
        {
            return "Support other plugins to make menu item or hotkey easily";
        }

        public IEnumerator Process(LanotaliumContext context)
        {
            InterfaceCreatorManager creator = new InterfaceCreatorManager();
            InterfaceHotKeyManager hotkey = new InterfaceHotKeyManager();

            //Load Menu Associated plugins
            foreach (var file in Directory.GetFiles(Application.dataPath + "/StreamingAssets/Plugins"))
            {
                Assembly assembly = Assembly.LoadFrom(file);
                Type[] types = assembly.GetTypes();
                foreach (var type in types)
                {
                    //Creator Buttons
                    if(type.GetInterface("ICreatorButton") != null)
                    {
                        ICreatorButton button = Activator.CreateInstance(type) as ICreatorButton;
                        creator.AddButton(button);
                    }
                    else if (type.GetInterface("ICreatorButtonHotKey") != null)
                    {
                        ICreatorButtonHotKey button = Activator.CreateInstance(type) as ICreatorButtonHotKey;
                        creator.AddButton(button);
                    }
                    else if (type.GetInterface("ICreatorButtonNumber") != null)
                    {
                        ICreatorButtonNumber button = Activator.CreateInstance(type) as ICreatorButtonNumber;
                        creator.AddButton(button);
                    }

                    if(type.GetInterface("IHotKey") != null)
                    {
                        IHotKey key = Activator.CreateInstance(type) as IHotKey;
                    }

                    creator.UpdateUI();
                }
            }

            

            while(true)
            {
                yield return null;
            }
        }
    }
}
