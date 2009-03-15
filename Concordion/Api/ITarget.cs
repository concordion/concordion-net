using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Concordion.Api
{
    public interface ITarget
    {
        void Write(Resource resource, String s);
        void CopyTo(Resource resource, Stream inputStream);
        void Delete(Resource resource);
    }
}
