using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Integration;

namespace Concordion.Spec.Concordion
{
    [ConcordionTest]
    public class ExampleTest
    {
        public string process(string html)
        {
            return new TestRig()
                .WithFixture(this)
                .Process(html)
                .SuccessOrFailureInWords()
                .ToLower();
        }

        public string getGreeting()
        {
            return "Hello World!";
        }
    }
}
