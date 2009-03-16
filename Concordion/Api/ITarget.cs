using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Concordion.Api
{
    public interface ITarget
    {
        void Write(Resource resource, string s);
        void CopyTo(Resource resource, string destination);
        void Delete(Resource resource);
    }
}
