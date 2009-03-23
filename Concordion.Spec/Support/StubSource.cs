using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Internal.Util;
using System.IO;

namespace Concordion.Spec
{
    class StubSource : ISource
    {
        private Dictionary<Resource, string> resources = new Dictionary<Resource, string>();

        public void AddResource(string resourceName, string content) 
        {
            AddResource(new Resource(resourceName), content);
        }

        public void AddResource(Resource resource, string content) 
        {
            resources.Add(resource, content);
        }

        #region ISource Members

        public System.IO.Stream CreateInputStream(Resource resource)
        {
            Check.IsTrue(CanFind(resource), "No such resource exists in simulator: " + resource.Path);
            return new MemoryStream(UTF8Encoding.UTF8.GetBytes(resources[resource]));
        }

        public TextReader CreateReader(Resource resource)
        {
            Check.IsTrue(CanFind(resource), "No such resource exists in simulator: " + resource.Path);
            return new StreamReader(new MemoryStream(UTF8Encoding.UTF8.GetBytes(resources[resource])));
        }

        public bool CanFind(Resource resource)
        {
            return resources.ContainsKey(resource);
        }

        #endregion
    }
}
