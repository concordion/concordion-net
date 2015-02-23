using System;
using System.Collections.Generic;
using Concordion.Runners.NUnit;
using Concordion.Spec.Support;
using NUnit.Framework;

namespace Concordion.Spec.Concordion.Integration
{
    [TestFixture]
    public class NUnitRunnerTest : Runners.NUnit.ExecutableSpecification
    {
        public bool GreetingsProcessed(string fragment)
        {
            return new TestRig()
                .WithFixture(this)
                .ProcessFragment(fragment)
                .IsSuccess;
        }

        public string GetGreeting()
        {
            return new Greeter().GetMessage();
        }
    }

    public class Greeter
    {
        public string GetMessage()
        {
            return "Hello World!";
        }
    }
}
