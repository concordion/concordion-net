using System;
using Concordion.Api;
using System.Reflection;
using System.IO;

namespace Concordion.Internal
{
    public class EmbeddedResourceSource : ISource
    {
        #region Properties

        public Assembly FixtureAssembly
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public EmbeddedResourceSource(Assembly fixtureAssembly)
        {
            this.FixtureAssembly = fixtureAssembly;
        }

        #endregion

        #region Methods

        private string ConvertPathToNamespace(string path)
        {
            var dottedPath = path.Replace('\\', '.');
            if (dottedPath[0] == '.')
            {
                dottedPath = dottedPath.Remove(0, 1);
            }
            return dottedPath;
        }

        #endregion

        #region ISource Members

        public TextReader CreateReader(Resource resource)
        {
            var fullyQualifiedTypeName = ConvertPathToNamespace(resource.Path);

            if (CanFind(resource))
            {
                return new StreamReader(FixtureAssembly.GetManifestResourceStream(fullyQualifiedTypeName));
            }
            
            throw new InvalidOperationException(String.Format("Cannot open the resource {0}", fullyQualifiedTypeName));
        }

        public bool CanFind(Resource resource)
        {
            var fullyQualifiedTypeName = ConvertPathToNamespace(resource.Path);
            return FixtureAssembly.GetManifestResourceInfo(fullyQualifiedTypeName) != null;
        }

        #endregion
    }
}
