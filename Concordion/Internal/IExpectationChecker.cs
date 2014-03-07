using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Internal
{
    public interface IExpectationChecker
    {
        bool IsAcceptable(string expected, object actual);
    }
}
