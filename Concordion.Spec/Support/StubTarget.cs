using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Internal.Util;

namespace Concordion.Spec
{
    class StubTarget : ITarget
    {
        private readonly Dictionary<Resource, String> writtenStrings;

        public StubTarget()
        {
            writtenStrings = new Dictionary<Resource, string>();
        }

        #region ITarget Members

        public void Write(Resource resource, string s)
        {
            writtenStrings.Add(resource, s);
        }

        public void CopyTo(Resource resource, string destination)
        {
        }

        public void Delete(Resource resource)
        {
        }

        #endregion

        public string GetWrittenString(Resource resource)
        {
            Check.IsTrue(writtenStrings.ContainsKey(resource), "Expected resource '" + resource.Path + "' was not written to target");
            return writtenStrings[resource];
        }
    }
}
