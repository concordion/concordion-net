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
using Concordion.Api;
using System.Reflection;
using System.IO;

namespace Concordion.Internal
{
    /// <summary>
    /// Loads the configuration file for a specification assembly
    /// </summary>
    public class SpecificationConfig
    {
        #region Properties

        /// <summary>
        /// Gets or sets the base input directory.
        /// </summary>
        /// <value>The base input directory.</value>
        public string BaseInputDirectory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the base output directory.
        /// </summary>
        /// <value>The base output directory.</value>
        public string BaseOutputDirectory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the suffix to be used for specification files.
        /// </summary>
        /// <value>The file suffix of specification documents (e.g. "html").</value>
        public List<string> SpecificationFileExtensions { get; set; }

        #endregion

        #region Constructors

        public SpecificationConfig()
        {
            this.BaseInputDirectory = Directory.GetCurrentDirectory();
            this.BaseOutputDirectory = Environment.GetEnvironmentVariable("TEMP");
            this.SpecificationFileExtensions = new List<string> {"html"};
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public SpecificationConfig Load(Type type)
        {
            Load(type.Assembly);
            return this;
        }

        /// <summary>
        /// Loads the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public SpecificationConfig Load(Assembly assembly)
        {
            Load(assembly.Location);
            return this;
        }

        /// <summary>
        /// Loads the specified path to assembly.
        /// </summary>
        /// <param name="pathToAssembly">The path to assembly.</param>
        /// <returns></returns>
        private SpecificationConfig Load(string pathToAssembly)
        {
            var configFileName = Path.ChangeExtension(pathToAssembly, ".config");
            if (File.Exists(configFileName))
            {
                var specificationConfigParser = new SpecificationConfigParser(this);
                specificationConfigParser.Parse(new StreamReader(configFileName));
            }

            return this;
        }

        #endregion

    }
}
