// Copyright 2009 Jeffrey Cameron
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Concordion.Api;
using System.IO;

namespace Concordion.Internal
{
    /// <summary>
    /// 
    /// </summary>
    public class SpecificationConfigParser
    {
        #region Properties
        
        /// <summary>
        /// Gets or sets the config.
        /// </summary>
        /// <value>The config.</value>
        public SpecificationConfig Config
        {
            get;
            private set;
        } 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecificationConfigParser"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public SpecificationConfigParser(SpecificationConfig config)
        {
            this.Config = config;
        } 

        #endregion

        /// <summary>
        /// Parses the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void Parse(TextReader reader)
        {
            var document = XDocument.Load(reader);
            LoadConfiguration(document);
        }

        /// <summary>
        /// Loads the configuration.
        /// </summary>
        /// <param name="document">The document.</param>
        private void LoadConfiguration(XDocument document)
        {
            var configElement = document.Root;

            if (configElement.Name == "Specification")
            {
                LoadBaseInputDirectory(configElement);
                LoadBaseOutputDirectory(configElement);
            }
        }

        /// <summary>
        /// Loads the base output directory.
        /// </summary>
        /// <param name="element">The element.</param>
        private void LoadBaseOutputDirectory(XElement element)
        {
            var baseOutputDirectory = element.Element("BaseOutputDirectory");

            if (baseOutputDirectory != null)
            {
                var pathAttribute = baseOutputDirectory.Attribute("path");

                if (pathAttribute != null)
                {
                    Config.BaseOutputDirectory = pathAttribute.Value;
                }
            }
        }

        /// <summary>
        /// Loads the base input directory.
        /// </summary>
        /// <param name="element">The element.</param>
        private void LoadBaseInputDirectory(XElement element)
        {
            var baseInputDirectory = element.Element("BaseInputDirectory");

            if (baseInputDirectory != null)
            {
                var pathAttribute = baseInputDirectory.Attribute("path");

                if (pathAttribute != null)
                {
                    Config.BaseInputDirectory = pathAttribute.Value;
                }
            }
        }
    }
}
