using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SamplePlugin.App
{
    public static class PluginUtils
    {
        public static IList<Assembly> FindPluginAssemblies()
        {
            List<Assembly> allAssemblies = new List<Assembly>();
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "plugins");

            foreach (string dll in Directory.GetFiles(path, "*.dll"))
            {
                allAssemblies.Add(Assembly.LoadFile(dll));
            }

            return allAssemblies;
        }

        public static void SetupPluginAssemblyPath()
        {
            string pluginsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "plugins");
            string rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler((sender, args) =>
            {
                var loadedAssembly = currentDomain.GetAssemblies().Where(x => x.FullName.Equals(args.Name)).FirstOrDefault();
                if (loadedAssembly != null)
                {
                    return loadedAssembly;
                }

                string assemblyPath = Path.Combine(rootPath, new AssemblyName(args.Name).Name + ".dll");
                if (File.Exists(assemblyPath))
                {
                    return Assembly.LoadFrom(assemblyPath);
                }
                else
                {
                    assemblyPath = Path.Combine(pluginsPath, new AssemblyName(args.Name).Name + ".dll");
                    if (File.Exists(assemblyPath))
                    {
                        return Assembly.LoadFrom(assemblyPath);
                    }
                }

                return null;
            });

        }
    }
}
