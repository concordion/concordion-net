using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            return path.Replace('\\', '.');
        }

        #endregion

        #region ISource Members

        public System.IO.TextReader CreateReader(Resource resource)
        {
            var path = ConvertPathToNamespace(resource.Path);
            return new StreamReader(FixtureAssembly.GetManifestResourceStream(path));
        }

        public bool CanFind(Resource resource)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
