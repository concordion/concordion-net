using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api
{
    public interface ISpecificationLocator
    {
        Resource LocateSpecification(object fixture);
    }
}
