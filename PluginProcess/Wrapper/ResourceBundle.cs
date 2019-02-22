using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Lanotalium.Plugin.Simple.Wrapper
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ResourceNameAttribute : Attribute
    {
        public string Name;

        public ResourceNameAttribute(string name)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class UseSubAssetAttribute : Attribute
    {
    }

    public class ResourceBundle
    {
        public static IEnumerator LoadFromBundle<T>(string path, System.Action<T> result, bool dependenciesExist = false)
        {
            T resources = Activator.CreateInstance<T>();

            var Bundle = AssetBundle.LoadFromFile(path);
            if (Bundle != null)
            {
                /*
                if(dependenciesExist)
                {
                    var req = Bundle.LoadAssetAsync<AssetBundleManifest>("AssetBundleManifest");
                    yield return req;

                    AssetBundleManifest manifest = req.asset as AssetBundleManifest;
                    string[] dependencies = manifest.GetAllAssetBundles();

                    File.WriteAllLines("filetesttt.txt", dependencies);
                    foreach (string dependency in dependencies)
                    {
                        yield return AssetBundle.LoadFromFileAsync(Path.Combine(Path.GetPathRoot(path), dependency));
                    }
                }*/
                

                var fields = resources.GetType().GetFields();
                foreach (var field in fields)
                {
                    var attr = field.GetCustomAttribute(typeof(ResourceNameAttribute));
                    if (attr is ResourceNameAttribute)
                    {
                        ResourceNameAttribute a = (ResourceNameAttribute)attr;
                        if (field.GetCustomAttribute(typeof(UseSubAssetAttribute)) is ResourceNameAttribute)
                        {
                            field.SetValue(resources, (Bundle.LoadAssetWithSubAssets(a.Name, field.FieldType)));
                        }
                        var req = Bundle.LoadAssetAsync(a.Name, field.FieldType);
                        yield return req;
                        field.SetValue(resources, req.asset);
                    }
                }
                Bundle.Unload(false);

                result(resources);
                yield return null;
            }
            yield return null;
        }
    }
}
