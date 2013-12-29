using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal
{
    public class FileTargetWithSuffix : ITarget
    {
        public FileTargetWithSuffix(string fileExtension)
        {
            
        }

        public void Write(Resource resource, string source)
        {
            throw new NotImplementedException();
        }

        public void Write(Resource resource, Bitmap image)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Resource resource, string destination)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Resource resource, TextReader inputReader)
        {
            throw new NotImplementedException();
        }

        public void Delete(Resource resource)
        {
            throw new NotImplementedException();
        }
    }
}
