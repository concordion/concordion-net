using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using System.IO;

namespace Concordion.Internal
{
    public class FileTarget : ITarget
    {
        #region Properties

        public string GetFile(Resource resource)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITarget Members

        public void Write(Resource resource, string s)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Resource resource, System.IO.Stream inputStream)
        {
            throw new NotImplementedException();
        }

        public void Delete(Resource resource)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
