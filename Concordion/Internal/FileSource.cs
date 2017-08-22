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
            this.BaseDirectory = Path.GetFullPath(baseDirectory);
        }

        #endregion

        #region ISource Members

        public TextReader CreateReader(Resource resource)
        {
            return new StreamReader(new FileStream(ExistingFilePath(resource), FileMode.Open));
        }

        public bool CanFind(Resource resource)
        {
            return ExistingFilePath(resource) != null;
        }

        #endregion

        #region private methods

        private string ExistingFilePath(Resource resource)
        {
	    //Console.WriteLine("== EXISTING FILE PATH ==");
	    //Console.WriteLine(BaseDirectory);
	    //Console.WriteLine(resource.Path);
            var filePath = Path.Combine(BaseDirectory, resource.Path);
            if (File.Exists(filePath))
            {
                return filePath;
            }
            filePath = Path.Combine(BaseDirectory, resource.ReducedPath);
            if (File.Exists(filePath))
            {
                return filePath;
            }
            return null;
        }

        #endregion
    }
}
