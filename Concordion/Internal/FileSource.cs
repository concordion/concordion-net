using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using System.IO;

namespace Concordion.Internal
{
    public class FileSource : ISource
    {
        #region Properties

        private string BaseDirectory
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public FileSource(string baseDirectory)
        {
            this.BaseDirectory = System.IO.Path.GetFullPath(baseDirectory);
        }

        #endregion

        #region ISource Members

        public System.IO.TextReader CreateReader(Resource resource)
        {
            return new StreamReader(new FileStream(Path.Combine(BaseDirectory, resource.Path), FileMode.Open));
        }

        public bool CanFind(Resource resource)
        {
            return File.Exists(Path.Combine(BaseDirectory, resource.Path));
        }

        #endregion
    }
}
