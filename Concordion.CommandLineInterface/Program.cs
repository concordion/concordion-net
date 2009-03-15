using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Internal;

namespace Concordion.CommandLineInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            Resource resource = new Resource(@"C:\Dev\concordion-1.3.1-RC4\src\test\resources\spec\examples\demo.html");
            Concordion concordion = new ConcordionBuilder().Build();
            IResultSummary resultSummary = concordion.Process(resource);
            resultSummary.Print(Console.Out, null);
            resultSummary.AssertIsSatisfied(null);
        }
    }
}
