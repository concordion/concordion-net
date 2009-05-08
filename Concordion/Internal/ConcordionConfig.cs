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
using System.Reflection;
using System.IO;
using Concordion.Api;

namespace Concordion.Internal
{
    /// <summary>
    /// 
    /// </summary>
    public class ConcordionConfig
    {
        #region Properties
        
        /// <summary>
        /// Gets or sets the runners.
        /// </summary>
        /// <value>The runners.</value>
        public Dictionary<string, IRunner> Runners
        {
            get;
            set;
        } 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcordionConfig"/> class.
        /// </summary>
        public ConcordionConfig()
        {
            Runners = new Dictionary<string, IRunner>();
        } 

        #endregion

        #region Methods
        
        /// <summary>
        /// Loads this instance.
        /// </summary>
        public ConcordionConfig Load()
        {
            var defaultConfigFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Concordion.config";
            Load(defaultConfigFile);

            return this;
        }

        /// <summary>
        /// Loads the specified config path.
        /// </summary>
        /// <param name="configPath">The config path.</param>
        public ConcordionConfig Load(string configPath)
        {
            if (File.Exists(configPath))
            {
                var parser = new ConcordionConfigParser(this);
                parser.Parse(new StreamReader(configPath));
            }

            return this;
        } 

        #endregion
    }
}
