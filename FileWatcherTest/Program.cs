using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherTest
{
    class Program
    {
        static void Main(string[] args)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = @"C:\TEMP";
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "*.*";

            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);

            watcher.EnableRaisingEvents = true;

            var cki = Console.ReadKey();
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("****" + e.ChangeType.ToString() + "***");
            Console.WriteLine("File: \"" + e.FullPath + "\" \"" + ToString(e));
            Console.WriteLine(ToString(source));

            DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(e.FullPath));
            FileInfo[] fiArray = di.GetFiles(e.Name, SearchOption.TopDirectoryOnly);
            if (fiArray == null || fiArray.Length == 0)
            {
                DirectoryInfo[] diArray = di.GetDirectories(e.Name, SearchOption.TopDirectoryOnly);
                if (diArray != null && diArray.Length == 1)
                {
                    Console.WriteLine(ToString(diArray[0]));
                }
            }
            else
            {
                Console.WriteLine(ToString(fiArray[0]));
            }

            Console.WriteLine("***\r\n");
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            Console.WriteLine("*** Rename ***");
            Console.WriteLine("\"{0}\" renamed to \"{1}\"", e.OldFullPath, e.FullPath);
            Console.WriteLine(ToString(source));
            Console.WriteLine("***\r\n");
        }

        public static string ToString(object o)
        {
            if (o == null)
                throw new ArgumentNullException("Call ToSTring() with null parameters.");

            Type oType = o.GetType();

            string toString = oType.Name + " {\r\n";
            PropertyInfo[] oProperties = oType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo oPI in oProperties)
            {
                toString += string.Format("\t{0} = {1}\r\n", oPI.Name, oPI.GetValue(o, null));
            }
            toString += "}";
            return toString;
        }
    }
}
