using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Concordion.Api
{
    public interface IResultSummary
    {
        long SuccessCount { get; } 
        long FailureCount { get; }
        long ExceptionCount { get; }
        bool HasExceptions { get; }
        bool HasFailures { get; }
        void AssertIsSatisfied(object fixture);
        void Print(TextWriter writer, object fixture);
    }
}
