using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Internal
{
    interface IExpressionValidator
    {
        void Validate(string expression);
    }
}
