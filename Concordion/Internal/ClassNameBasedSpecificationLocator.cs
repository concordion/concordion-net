using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal
{
    public class ClassNameBasedSpecificationLocator : ISpecificationLocator
    {
        #region ISpecificationLocator Members

        public Resource LocateSpecification(object fixture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
