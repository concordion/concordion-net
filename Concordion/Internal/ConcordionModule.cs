using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Ninject.Modules;
using System.Collections;
using Concordion.Internal.Commands;

namespace Concordion.Internal
{
    class ConcordionModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<IExecuteStrategy>().ToProvider();
        }
    }
}
