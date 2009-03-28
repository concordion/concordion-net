using System;
using Concordion.Internal.Commands;
namespace Concordion.Internal.Renderer
{
    public interface IExceptionListener
    {
        void ExceptionCaughtEventHandler(object sender, ExceptionCaughtEventArgs e);
    }
}
