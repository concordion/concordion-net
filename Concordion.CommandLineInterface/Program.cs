using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Internal;
using Concordion.Integration;
using System.IO;

namespace Concordion.CommandLineInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseDirectory = args[0];
            string assemblyDirectory = args[1];
            var assemblies = new List<string> { assemblyDirectory };

            var resourcePaths = new ResourceCrawler(baseDirectory);
            var fixtureDiscoverer = new FixtureDiscoverer();
            fixtureDiscoverer.LoadAssemblies(assemblies);

            foreach (string resourcePath in resourcePaths)
            {
                var resource = new Resource(resourcePath);
                var fixture = fixtureDiscoverer.GetFixture(resource);
                IResultSummary resultSummary = new ConcordionBuilder().SendOutputTo(baseDirectory + "Results").Build().Process(resource, fixture);
                resultSummary.Print(System.Console.Out, fixture);
                resultSummary.AssertIsSatisfied(fixture);
            }
        }
    }
}
