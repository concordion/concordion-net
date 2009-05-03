using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using Concordion.Api;

namespace Concordion.Internal
{
    public class ConcordionConfigParser
    {
        public ConcordionConfig Config
        {
            get;
            private set;
        }

        public ConcordionConfigParser(ConcordionConfig config)
        {
            this.Config = config;
        }

        public void Parse(TextReader reader)
        {
            var document = XDocument.Load(reader);
            LoadConfiguration(document);
        }

        private void LoadConfiguration(XDocument document)
        {
            var configElement = document.Root;

            if (configElement.Name == "Concordion")
            {
                LoadBaseInputDirectory(configElement);
                LoadBaseOutputDirectory(configElement);
                LoadSpecificationAssemblies(configElement);
                LoadRunners(configElement);
            }
        }

        private void LoadRunners(XElement element)
        {
            var runners = element.Element("Runners");

            if (runners != null)
            {
                foreach (var runner in runners.Elements("Runner"))
                {
                    var alias = runner.Attribute("alias");
                    var runnerTypeText = runner.Attribute("type");

                    if (alias != null && runnerTypeText != null)
                    {
                        var runnerType = Type.GetType(runnerTypeText.Value);

                        if (runnerType != null)
                        {
                            var runnerObject = Activator.CreateInstance(runnerType);
                            if (runnerObject != null && runnerObject is IRunner)
                            {
                                Config.Runners.Add(alias.Value, runnerObject as IRunner);
                            }
                        }
                    }
                }
            }
        }

        private void LoadSpecificationAssemblies(XElement element)
        {
            var specificationAssemblies = element.Element("SpecificationAssemblies");

            if (specificationAssemblies != null)
            {
                foreach (var specificationAssembly in specificationAssemblies.Elements("SpecificationAssembly"))
                {
                    this.Config.SpecificationAssemblies.Add(specificationAssembly.Value);
                }
            }
        }

        private void LoadBaseOutputDirectory(XElement element)
        {
            var baseOutputDirectory = element.Element("BaseOutputDirectory");

            if (baseOutputDirectory != null)
            {
                Config.BaseOutputDirectory = baseOutputDirectory.Value;
            }
        }

        private void LoadBaseInputDirectory(XElement element)
        {
            var baseInputDirectory = element.Element("BaseInputDirectory");

            if (baseInputDirectory != null)
            {
                Config.BaseInputDirectory = baseInputDirectory.Value;
            }
        }
    }
}