using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Concordion.Api
{
    public interface ISource
    {
        TextReader CreateReader(Resource resource);
        bool CanFind(Resource resource);
    }
}
