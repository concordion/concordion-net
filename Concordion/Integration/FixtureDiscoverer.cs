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

namespace Concordion.Integration
{
    public class FixtureDiscoverer
    {
        #region Properties

        private List<Assembly> FixtureAssemblies
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public FixtureDiscoverer()
        {
            FixtureAssemblies = new List<Assembly>();

            FixtureAssemblies.Add(Assembly.GetExecutingAssembly());
        }

        #endregion

        #region Methods

        public void LoadAssemblies(ICollection<string> assemblyPaths)
        {
            foreach (var assemblyPath in assemblyPaths)
            {
                var assembly = Assembly.LoadFile(assemblyPath);
                FixtureAssemblies.Add(assembly);
            }
        }

        public object GetFixture(Resource resource)
        {
            string fixtureHtmlName = resource.Name;
            string[] parts = fixtureHtmlName.Split('\\');
            string fixtureName = parts[parts.Length - 1].Replace(".html", "Test");

            foreach (Assembly assembly in FixtureAssemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsClass && type.Name == fixtureName)
                    {
                        ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);

                        if (constructor != null)
                        {
                            return constructor.Invoke(new Object[] { });
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }

            return null;
        }


        #endregion
    }
}
