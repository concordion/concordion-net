using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Command.Execute
{
    [ConcordionTest]
    public class ListExampleTest
    {
        public void Print(string message)
        {
            Console.WriteLine(message);
        }
    }
}
