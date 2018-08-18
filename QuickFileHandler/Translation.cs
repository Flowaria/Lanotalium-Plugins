using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lanotalium.Plugin.Files
{
    [FileName("lanotalium.plugin")]
    public class Example
    {
        [NodeName("ui.goodtek")]
        public string GoodTek;
    }



    public class Translation
    {
        public static string DefaultLanguage = "en";
        public static List<string> SupportedLanguage = new List<string>();

        public const string TRANSLATION_FOLDER = "Translation";
        public const string FORMAT_FILEPATH = "{0}.{1}.txt";

        public static T InitTranslation<T>(string language = "default")
        {
            if (language == "default")
                language = DefaultLanguage;

            string filename = typeof(T).Name;

            var attrs = Attribute.GetCustomAttributes(typeof(T));
            foreach (var attr in attrs)
            {
                if (attr is FileNameAttribute)
                {
                    FileNameAttribute a = (FileNameAttribute)attr;
                    filename = a.FileName;
                }
            }

            string filepath = String.Join("/", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), TRANSLATION_FOLDER);
            string def_filepath = Path.Combine(filepath, String.Format(FORMAT_FILEPATH, language, filename));

            T translated = default(T);
            if ((translated = LoadTranslation<T>(def_filepath)) == null)
            {
                WriteTranslation<T>(Path.Combine(filepath, String.Format(FORMAT_FILEPATH, language, filename)));
                foreach (var lang in SupportedLanguage)
                    WriteTranslation<T>(Path.Combine(filepath, String.Format(FORMAT_FILEPATH, lang, filename)));

                return (T)(Activator.CreateInstance(typeof(T)));
            }
            else
            {
                return translated;
            }
        }

        private static void ReadConfig()
        {
            string filepath = Path.GetFullPath(String.Join("/", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), TRANSLATION_FOLDER, "default.txt"));
            if(File.Exists(filepath))
            {
                var content = File.ReadAllLines(filepath);
                if(content.Length >= 2)
                {
                    if (content[0].StartsWith("default: "))
                    {
                        var lang_def = content[0].Replace("default: ","");
                        DefaultLanguage = lang_def;
                    }
                    if (content[1].StartsWith("available: "))
                    {
                        var langs = content[1].Replace("available: ", "").Split('|');
                        SupportedLanguage.AddRange(langs);
                    }
                }
            }
        }

        public static T LoadTranslation<T>(string filepath)
        {
            if (File.Exists(filepath))
            {
                Dictionary<string, string> Nodes = new Dictionary<string, string>();
                var content = File.ReadAllLines(filepath);
                foreach (var line in content)
                {
                    var node = line.Split('$');
                    if (node.Length == 2)
                    {
                        Nodes.Add(node[0], node[1]);
                    }
                }

                int count = Nodes.Count;

                T translated = (T)(Activator.CreateInstance(typeof(T)));
                var properties = translated.GetType().GetProperties();
                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(string))
                    {
                        var attr = property.GetCustomAttribute(typeof(NodeNameAttribute));
                        if (attr is NodeNameAttribute)
                        {
                            NodeNameAttribute a = (NodeNameAttribute)attr;
                            if (Nodes.ContainsKey(a.NodeName))
                            {
                                property.SetValue(translated, Nodes[a.NodeName]);
                                count--;
                            }
                        }
                        else
                        {
                            if(Nodes.ContainsKey(property.Name))
                            {
                                property.SetValue(translated, Nodes[property.Name]);
                                count--;
                            }
                        }
                    }
                }

                if (count == 0)
                    return translated;
            }
            return default(T);
        }

        public static bool WriteTranslation<T>(string filepath)
        {
            if (!File.Exists(filepath))
            {
                using (var writer = new StreamWriter(filepath, true))
                {
                    T def = (T)(Activator.CreateInstance(typeof(T)));
                    var properties = def.GetType().GetProperties();
                    foreach (var property in properties)
                    {
                        if (property.PropertyType == typeof(string))
                        {
                            var attr = property.GetCustomAttribute(typeof(NodeNameAttribute));
                            if (attr is NodeNameAttribute)
                            {
                                NodeNameAttribute a = (NodeNameAttribute)attr;
                                writer.Write(a.NodeName);
                            }
                            else
                            {
                                writer.Write(property.Name);
                            }
                            writer.Write('$');

                            var value = property.GetValue(def);
                            if (value != null)
                            {
                                writer.WriteLine((string)value);
                            }
                            else
                            {
                                writer.Dispose();
                                if (File.Exists(filepath))
                                    File.Delete(filepath);

                                return false;
                            }
                        }
                    }
                    writer.Flush();
                    return true;
                }
            }
            return false;
        }
    }
}