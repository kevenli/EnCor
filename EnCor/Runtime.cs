using System;
using System.Collections.Generic;
using EnCor.Configuration;
using EnCor.Logging;
using EnCor.Logging.Appenders;
using EnCor.ObjectBuilder;
using EnCor.Security;
using System.IO;
using EnCor.ModuleLoader;
using System.Reflection;

namespace EnCor
{
    /// <summary>
    /// Entrance of EnCor environment.
    /// Start/stop runtime, get EnCor interfaces from here.
    /// </summary>
    public sealed class Runtime
    {
        private static Runtime _instance; // singleton instance
        private static readonly object InstanceLock = new object();//locker for instance building.

        // service container to hold all services in modules;
        private IServiceContainer _serviceContainer;

        private IList<ModuleInfo> _modules = new List<ModuleInfo>();

        private static IList<string> _assemblyLookupPaths = new List<string>();

        /// <summary>
        /// Startup the EnCor runtime, use the encor.config configuration file.
        /// </summary>
        public static void Startup()
        {
            var defaultFileConfig = FileEnCorConfig.GetConfig();
            Startup(defaultFileConfig);
        }

        static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            foreach (var dir in _assemblyLookupPaths)
            {
                string assemblyPath = Path.Combine(dir, new AssemblyName(args.Name).Name + ".dll");
                if (File.Exists(assemblyPath))
                {
                    Assembly assembly = Assembly.LoadFrom(assemblyPath);
                    return assembly;
                }
            }

            return null;
        }

        internal static void InitInSubDomain(IServiceContainer serviceContainer)
        {
            if (_instance == null && AppDomain.CurrentDomain.IsDefaultAppDomain())
            {
                _instance = new Runtime(serviceContainer);
            }
        }

        /// <summary>
        /// Startup the EnCor runtime
        /// </summary>
        public static void Startup(IEnCorConfig config)
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            config.Verify();
            if (_instance != null)
            {
                throw new EnCorException("EnCor runtime has been started.");
            }
            lock (InstanceLock)
            {
                if (_instance == null)
                {
                    lock (InstanceLock)
                    {
                        try
                        {
                            IServiceContainer serviceContainer = new ServiceContainer();
                            _instance = new Runtime(serviceContainer);

                            _instance.Start(config.Modules);
                        }
                        catch (Exception ex)
                        {
                            if (_instance != null)
                            {
                                foreach (var module in _instance._modules)
                                {
                                    module.Initializer.Stop();
                                }
                            }
                            Logging.Error("Error when starting runtime", ex);
                            _instance = null;
                            throw;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Stop the EnCor runtime
        /// </summary>
        public static void Stop()
        {
            if (_instance != null)
            {
                _instance.StopInstance();
                _instance = null;
            }
        }

        private Runtime(IServiceContainer serviceContainer)
        {
            _serviceContainer = serviceContainer;
        }

        internal static IServiceContainer GetServiceContainer()
        {
            return _instance._serviceContainer;
        }

        private void Start(IList<IModuleConfig> modules)
        {
            if (modules.Count > 0)
            {
                IList<IModuleConfig> sortedModuleList = new List<IModuleConfig>();
                foreach (IModuleConfig moduleConfig in modules)
                {
                    ModuleDependentSorter.SortModuleByDependency(moduleConfig, ref sortedModuleList, modules);
                }

                foreach (IModuleConfig module in sortedModuleList)
                {
                    // subdomain
                    //AppDomain moduleDomain = CreateModuleDomain(moduleName);
                    //ModuleInitializer moduleInitializer = (ModuleInitializer)moduleDomain.CreateInstanceAndUnwrap("EnCor", "EnCor.ModuleLoader.ModuleInitializer");

                    AppDomain moduleDomain = AppDomain.CurrentDomain;
                    ModuleInitializer moduleInitializer = new ModuleInitializer();
                    ShadowCopy(module.ModuleName);
                    moduleInitializer.SetModuleConfig(module);
                    moduleInitializer.VefiryConfig();
                    moduleInitializer.Init(_serviceContainer);

                    //foreach (object serviceInstance in moduleInitializer.GetAllServices())
                    //{
                    //    RegisterService(moduleName, serviceInstance);
                    //}
                    //moduleInitializer.Start();
                    ModuleInfo moduleInfo = new ModuleInfo(module.ModuleName, moduleDomain, moduleInitializer);
                    _modules.Add(moduleInfo);
                }

                foreach (var module in _modules)
                {
                    module.Initializer.Start();
                }
            }
        }

        private void ShadowCopy(string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName))
            {
                throw new EnCorException("Module name cannot be empty");
            }
            string modulePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"modules\{0}\bin", moduleName));
            if (Directory.Exists(modulePath))
            {
                string moduleShadowPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"shadow\{0}\bin", moduleName));
                if (!Directory.Exists(moduleShadowPath))
                {
                    Directory.CreateDirectory(moduleShadowPath);
                }
                foreach (var file in Directory.GetFiles(modulePath))
                {
                    File.Copy(file, moduleShadowPath + "\\" + Path.GetFileName(file), true);
                }
                _assemblyLookupPaths.Add(moduleShadowPath);
            }
        }

        private AppDomain CreateModuleDomain(string moduleName)
        {
            AppDomainSetup appSetup = new AppDomainSetup()
            {
                ApplicationName = moduleName,
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                PrivateBinPath = string.Format(@"shadow\{0}\bin", moduleName),
                ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
            };
            AppDomain domain = AppDomain.CreateDomain(moduleName, null, appSetup);
            return domain;
        }

        

        private void StopInstance()
        {
            foreach (var module in _modules)
            {
                module.Initializer.Stop();
            }
        }

        public static ISecurity Security
        {
            get
            {
                return GetService<ISecurity>();
            }
        }

        public static ILogging Logging
        {
            get
            {
                return GetService<ILogging>()??LoggingFactory.GetLogging();
            }
        }

        public static T GetService<T>()
        {
            return (T)_instance._serviceContainer.GetService(typeof(T));
        }

        public static object GetService(string uniqueName)
        {
            return _instance._serviceContainer.GetService(uniqueName);
        }

        public static Type GetServiceInterface(object serviceInstance)
        {
            return _instance._serviceContainer.GetServiceInterface(serviceInstance);
        }
    }
}
