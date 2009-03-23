using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using System.IO;

namespace Concordion.Internal
{
    public class ClassPathSource : ISource
    {
        #region ISource Members

        public System.IO.Stream CreateInputStream(Resource resource)
        {
            return new FileStream(resource.Path, FileMode.Open);
        }

        public TextReader CreateReader(Resource resource)
        {
            return new StreamReader(new FileStream(resource.Path, FileMode.Open));
        }

        public bool CanFind(Resource resource)
        {
            return File.Exists(resource.Path) ? true : false;
        }

        #endregion
    }
}
