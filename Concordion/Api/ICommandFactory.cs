using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api
{
    public interface ICommandFactory
    {
        ICommand CreateCommand(string namespaceUri, string commandName);
    }
}
