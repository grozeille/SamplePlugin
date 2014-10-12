using NuGet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SamplePlugin.App
{
    public static class PluginUtils
    {
        public static void DownloadAndInstallPlugins(String repositoryPath)
        {
            var applicationRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (!Path.IsPathRooted(repositoryPath))
            {
                var directory = new DirectoryInfo(repositoryPath);
                if (!directory.Exists)
                {
                    Console.WriteLine("Warning: this path does not exist: " + directory.FullName);
                }
                repositoryPath = directory.FullName;
            }

            Console.WriteLine("Searching for plugins in repository: " + repositoryPath);
            Console.WriteLine();
            var pluginRepository = PackageRepositoryFactory.Default.CreateRepository(repositoryPath);

            // filter only plugins
            var allPlugins = pluginRepository.GetPackages()
                                .Where(p => p.Tags.Contains("SamplePlugin") && p.Tags.Contains("Plugin"))
                                .OrderBy(p => p.Id)
                                .ToList();

            allPlugins.ForEach(x => Console.WriteLine("Found Plugin: " + x));
            Console.WriteLine();

            // download plugins with dependencies
            var repository = new AggregateRepository(new[] 
            {
                PackageRepositoryFactory.Default.CreateRepository("https://www.nuget.org/api/v2/"),
                pluginRepository
            });
            var localRepositoryPath = Path.Combine(applicationRootPath, "LocalRepository");
            var manager = new PackageManager(repository, localRepositoryPath);

            foreach (var plugin in allPlugins)
            {
                try
                {
                    Console.WriteLine("Installing " + plugin + "...");
                    manager.InstallPackage(plugin, false, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to install package " + plugin + Environment.NewLine + ex);
                }
            }
            Console.WriteLine();

            // copy plugin files into the plugin folder
            string pluginsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "plugins");
            var pluginsDirectory = new DirectoryInfo(pluginsPath);
            if (!pluginsDirectory.Exists)
            {
                pluginsDirectory.Create();
            }

            foreach (var localPackage in manager.LocalRepository.GetPackages())
            {
                string localPackageRootPath = Path.Combine(localRepositoryPath, localPackage.Id + "." + localPackage.Version);

                // retrieve the last supported framework
                var latestSupportedFramework = localPackage.GetSupportedFrameworks()
                    .Where(x => x.Identifier.Equals(".NETFramework") && x.Version.CompareTo(new Version(4, 5)) <= 0)
                    .OrderByDescending(x => x.Version).FirstOrDefault();

                foreach (var lib in localPackage.GetLibFiles().Where(x => x.TargetFramework == latestSupportedFramework))
                {
                    File.Copy(Path.Combine(localPackageRootPath, lib.Path), Path.Combine(pluginsPath, Path.GetFileName(lib.Path)), true);
                }
                foreach (var file in localPackage.GetContentFiles().Where(x => x.TargetFramework == latestSupportedFramework))
                {
                    File.Copy(Path.Combine(localPackageRootPath, file.Path), Path.Combine(pluginsPath, Path.GetFileName(file.Path)), true);
                }
            }
        }

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
