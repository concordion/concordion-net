using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using System.IO;
using Concordion.Internal.Util;

namespace Concordion.Internal
{
    public class FileTarget : ITarget
    {
        #region Fields

        private static readonly long FRESH_ENOUGH_MILLIS = 2000; // 2 secs

        #endregion

        #region Properties

        private string BaseDirectory
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public FileTarget(string baseDirectory)
        {
            BaseDirectory = baseDirectory;
        }
        
        #endregion

        #region Methods

        private void MakeDirectories(Resource resource)
        {
            // TODO - make the output directories here
            throw new NotImplementedException();
        }

        private StreamWriter CreateWriter(Resource resource)
        {
            string path = BaseDirectory + resource.Path;
            return new StreamWriter(path, false, Encoding.UTF8);
        }


        private bool IsFreshEnough(string source)
        {
            TimeSpan ageInMillis = DateTime.Now.Subtract(File.GetLastWriteTime(source));
            return ageInMillis.TotalMilliseconds < FRESH_ENOUGH_MILLIS;
        }

        #endregion

        #region ITarget Members

        public void Write(Resource resource, string s)
        {
            Check.NotNull(resource, "resource is null");
            MakeDirectories(resource);
            using (StreamWriter writer = CreateWriter(resource))
            {
                try
                {
                    writer.Write(s);
                }
                finally { }
            }
        }

        public void CopyTo(Resource resource, string destination)
        {
            Check.NotNull(resource, "resource is null");
            MakeDirectories(resource);
            string source = BaseDirectory + resource.Path;

            if (File.Exists(source) && IsFreshEnough(source))
            {
                return;
            }
            File.Copy(source, destination);
        }

        public void Delete(Resource resource)
        {
            Check.NotNull(resource, "resource is null");
            File.Delete(BaseDirectory + resource.Path);
        }

        #endregion
    }
}
