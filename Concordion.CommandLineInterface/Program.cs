// Copyright 2009 Jeffrey Cameron
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
            Console.Title = "Concordion.Net";
            string baseInputDirectory = args[0];
            string assemblyDirectory = args[1];
            string baseOutputDirectory = args[2];
            var assemblies = new List<string> { assemblyDirectory };

            var resourcePaths = new ResourceCrawler(baseInputDirectory);
            var fixtureDiscoverer = new FixtureDiscoverer();
            fixtureDiscoverer.LoadAssemblies(assemblies);

            var results = new Results();

            foreach (string resourcePath in resourcePaths)
            {
                var resource = new Resource(resourcePath);
                var fixture = fixtureDiscoverer.GetFixture(resource);
                if (fixture != null)
                {
                    IResultSummary resultSummary = new ConcordionBuilder()
                                                                .SendOutputTo(baseOutputDirectory)
                                                                .Build()
                                                                .Process(resource, fixture);
                    resultSummary.Print(System.Console.Out, fixture);
                    SumResults(results, resultSummary);
                    try
                    {
                        resultSummary.AssertIsSatisfied(fixture);
                    }
                    catch (ConcordionAssertionException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine("Concordion says ... Successes: {0}, Failures: {1}, Exceptions {2}", results.Success, results.Failure, results.Exception);
        }

        private static void SumResults(Results results, IResultSummary resultSummary)
        {
            results.Success += resultSummary.SuccessCount;
            results.Failure += resultSummary.FailureCount;
            results.Exception += resultSummary.ExceptionCount;
        }

        private class Results
        {
            public long Success;
            public long Failure;
            public long Exception;
        }
    }
}
