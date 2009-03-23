using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api
{
    public interface ISpecificationReader
    {
        ISpecification ReadSpecification(Resource resource);
        ISpecification ReadSpecification(Resource resource, string baseDirectory);
    }
}
