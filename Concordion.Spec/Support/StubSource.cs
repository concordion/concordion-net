using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Internal.Util;
using System.IO;

namespace Concordion.Spec.Support
{
    class StubSource : ISource
    {
        private Dictionary<Resource, string> resources = new Dictionary<Resource, string>();

        public void AddResource(string resourceName, string content) 
        {
            this.AddResource(new Resource(resourceName), content);
        }

        public void AddResource(Resource resource, string content) 
        {
            if (!this.resources.ContainsKey(resource))
            {
                this.resources.Add(resource, content);
            }
            else
            {
                this.resources.Remove(resource);
                this.resources.Add(resource, content);
            }
        }

        #region ISource Members

        public System.IO.Stream CreateInputStream(Resource resource)
        {
            Check.IsTrue(this.CanFind(resource), "No such resource exists in simulator: " + resource.Path);
            return new MemoryStream(UTF8Encoding.UTF8.GetBytes(this.resources[resource]));
        }

        public TextReader CreateReader(Resource resource)
        {
            Check.IsTrue(this.CanFind(resource), "No such resource exists in simulator: " + resource.Path);
            return new StreamReader(new MemoryStream(UTF8Encoding.UTF8.GetBytes(this.resources[resource])));
        }

        public bool CanFind(Resource resource)
        {
            return this.resources.ContainsKey(resource);
        }

        #endregion
    }
}
