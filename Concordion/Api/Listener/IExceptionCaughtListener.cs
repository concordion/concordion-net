using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api.Listener
{
    public interface IExceptionCaughtListener
    {
        void ExceptionCaught(ExceptionCaughtEvent caughtEvent);
    }
}
