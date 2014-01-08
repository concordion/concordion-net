using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Concordion.Api;
using Concordion.Internal.Util;
using System.Drawing;

namespace Concordion.Spec.Support
{
    class StubTarget : ITarget
    {
        private readonly Dictionary<Resource, String> writtenStrings;
        private readonly List<Resource> m_CopiedResources = new List<Resource>();

        public StubTarget()
        {
            this.writtenStrings = new Dictionary<Resource, string>();
        }

        public string GetWrittenString(Resource resource)
        {
            Check.IsTrue(this.writtenStrings.ContainsKey(resource), "Expected resource '" + resource.Path + "' was not written to target");
            return this.writtenStrings[resource];
        }

        #region ITarget Members

        public void Write(Resource resource, string s)
        {
            this.writtenStrings.Add(resource, s);
        }

        public void Write(Resource resource, Bitmap image)
        {
            // Do nothing
        }

        public void CopyTo(Resource resource, string destination)
        {
        }

        public void CopyTo(Resource resource, TextReader inputReader)
        {
            this.m_CopiedResources.Add(resource);
        }

        public bool HasCopiedResource(Resource resource)
        {
            return this.m_CopiedResources.Contains(resource);
        }

        public void Delete(Resource resource)
        {
        }

        public string ResolvedPathFor(Resource resource)
        {
            return "";
        }

        #endregion
    }
}
