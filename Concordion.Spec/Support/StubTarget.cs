using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Internal.Util;
using System.Drawing;

namespace Concordion.Spec
{
    class StubTarget : ITarget
    {
        private readonly Dictionary<Resource, String> writtenStrings;
        private readonly List<Resource> m_CopiedResources = new List<Resource>();

        public StubTarget()
        {
            writtenStrings = new Dictionary<Resource, string>();
        }

        public string GetWrittenString(Resource resource)
        {
            Check.IsTrue(writtenStrings.ContainsKey(resource), "Expected resource '" + resource.Path + "' was not written to target");
            return writtenStrings[resource];
        }

        #region ITarget Members

        public void Write(Resource resource, string s)
        {
            writtenStrings.Add(resource, s);
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
            m_CopiedResources.Add(resource);
        }

        public bool HasCopiedResource(Resource resource)
        {
            return m_CopiedResources.Contains(resource);
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
