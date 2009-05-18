using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Ninject.Parameters;

namespace Concordion.Internal
{
    public static class ObjectFactory
    {
        private static IKernel _kernel = new StandardKernel(new ConcordionModule());

        public static T Resolve<T>()
        {
            return _kernel.Get<T>();
        }

        public static T Resolve<T>(params IParameter[] parameters)
        {
            return _kernel.Get<T>(parameters);
        }
    }
}
