using System;
using System.Collections.Generic;
using System.Linq;

namespace Concordion.Api.Listener
{
    public interface IExecuteListener
    {
        void ExecuteCompleted(ExecuteEvent executeEvent);
    }
}
