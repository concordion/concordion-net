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
                            // TODO - type does not have a constructor with no parameters, cannot instantiate it
                            throw new InvalidOperationException(String.Format("", fixtureName));
                        }
                    }
                }
            }

            throw new InvalidOperationException("Searched all assemblies and cannot find an appropriate type for the fixture " + fixtureName);
        }


        #endregion
    }
}
