using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using EnCor.ModuleLoader;
using EnCor.Security;
using EnCor.ObjectBuilder;
using EnCor.Logging;
using System.Collections;
using System.IO;

namespace EnCor.Configuration
{
    public sealed class FileEnCorConfig : ConfigurationElementCollection, IEnCorConfig
    {
        private const string XmlStart = "enCor";
        private const string ConfigDataType = "dataType";
        private const string ConfigType = "type";

        //private readonly IList<ModuleConfig> _modules = new List<ModuleConfig>();
        private readonly Dictionary<string, IModuleConfig> _modules = new Dictionary<string, IModuleConfig>();

        /// <summary>
        /// Whether to lookup modules in the "modules" folder, it will cause module config lookup when verifying.
        /// </summary>
        private bool LookupModulesInSubFolders = false;

        private FileEnCorConfig(string filePath, bool lookupModules)
        {
            using (XmlReader reader = new XmlTextReader(filePath))
            {
                reader.ReadStartElement(XmlStart);
                DeserializeElement(reader, false);
            }
            LookupModulesInSubFolders = lookupModules;
        }

        public FileEnCorConfig(XmlReader reader)
        {
            reader.ReadStartElement(XmlStart);
            DeserializeElement(reader, false);
            reader.Close();
        }

        private FileEnCorConfig()
        {
            LookupModulesInSubFolders = true;
        }

        protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
        {
            ModuleConfig config = new ModuleConfig();
            config.DeserializeElement(reader);
            config.ModuleName = elementName;
            //IModuleConfig config = ModuleConfig.ParseConfig(reader, elementName);
            _modules.Add(config.ModuleName, config);
            return true;
        }

        #region IEnCorConfig Members

        public IList<IModuleConfig> Modules
        {
            get
            {
                return new List<IModuleConfig>(_modules.Values);
            }
        }

        #endregion

        protected override ConfigurationElement CreateNewElement()
        {
            return new ModuleConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModuleConfig)element).ModuleName;
        }

        public static FileEnCorConfig GetFileConfig(string path)
        {
            return new FileEnCorConfig(path, true);
        }

        public static IEnCorConfig GetConfig()
        {
            object execFileConfigSection = ConfigurationManager.GetSection("enCor");
            if (execFileConfigSection != null)
            {
                return (IEnCorConfig)execFileConfigSection;
            }

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "encor.config");
            if (File.Exists(path))
            {
                return GetFileConfig(path);
            }

            // if cannot find a config file, return a empty config
            FileEnCorConfig config = new FileEnCorConfig();
            return config;
        }

        public void Verify()
        {
            if (LookupModulesInSubFolders)
            {
                if (Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "modules")))
                {
                    foreach (var subDir in Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "modules")))
                    {
                        string moduleConfigFilePath = Path.Combine(subDir, "module.config");
                        if (File.Exists(moduleConfigFilePath))
                        {
                            IModuleConfig moduleConfig = ModuleConfig.ParseConfig(moduleConfigFilePath);
                            _modules.Add(moduleConfig.ModuleName, moduleConfig);
                        }
                    }
                }
            }
        }
    }
}
