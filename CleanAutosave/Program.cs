using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanAutosave
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Are you sure to delete all Autosave folder below this program?");
            Console.Write(String.Format("Path : {0} (Y/N): ", Directory.GetCurrentDirectory()));
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Y)
            {
                Console.WriteLine("\n===========");

                var list = DeleteAutosaveDirectory(Directory.GetCurrentDirectory(), new List<string>());
                Console.WriteLine("Found AutoSave Folder: "+list.Count);
                if(list.Count > 0)
                {
                    foreach (var path in list)
                    {
                        Console.WriteLine(path);
                    }
                    Console.WriteLine("Are you sure to delete it? (Y/N): ");
                    key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Y)
                    {
                        foreach (var path in list)
                        {
                            Directory.Delete(path, true);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Nothing found in your project folder");
                    Thread.Sleep(2000);
                }
            }
        }

        public static List<string> DeleteAutosaveDirectory(string path, List<string> list)
        {
            foreach(var cPath in Directory.GetDirectories(path))
            {
                if(Path.GetFileName(cPath).Equals("AutoSave"))
                {
                    list.Add(cPath);
                }
                else
                {
                    DeleteAutosaveDirectory(cPath, list);
                }
            }
            return list;
        }
    }
}
