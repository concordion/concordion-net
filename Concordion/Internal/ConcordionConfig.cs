using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using Concordion.Api;

namespace Concordion.Internal
{
    [Serializable]
    public class ConcordionConfig
    {
        public string BaseInputDirectory
        {
            get;
            set;
        }

        public string BaseOutputDirectory
        {
            get;
            set;
        }

        public List<string> SpecificationAssemblies
        {
            get;
            set;
        }

        public Dictionary<string, IRunner> Runners
        {
            get;
            set;
        }

        public ConcordionConfig()
        {
            SpecificationAssemblies = new List<string>();
            Runners = new Dictionary<string, IRunner>();
        }

        public void Load()
        {
            var defaultConfigFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\concordion.config";
            Load(defaultConfigFile);
        }

        public void Load(string configPath)
        {
            if (File.Exists(configPath))
            {
                var parser = new ConcordionConfigParser(this);
                parser.Parse(new StreamReader(configPath));
            }
        }
    }
}
