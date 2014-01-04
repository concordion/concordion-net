using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal
{
    public class FixtureRunner
    {
        public IResultSummary Run(object fixture)
        {
            try
            {
                var source = new EmbeddedResourceSource(fixture.GetType().Assembly);
                var specificationConfig = new SpecificationConfig().Load(fixture.GetType());
                var target = new FileTarget(specificationConfig.BaseOutputDirectory);

                var testSummary = new SummarizingResultRecorder();
                var specExtensions = specificationConfig.SpecificationFileExtensions;
                var anySpecExecuted = false;
                foreach (var specExtension in specExtensions)
                {
                    var specLocator = new ClassNameBasedSpecificationLocator(specExtension);
                    var specResource = specLocator.LocateSpecification(fixture);
                    if (source.CanFind(specResource))
                    {
                        var concordion = new ConcordionBuilder()
                            .WithSource(source)
                            .WithTarget(target)
                            .WithSpecificationLocator(specLocator)
                            .Build();
                        var fixtureResult = concordion.Process(fixture);
                        this.AddToTestResults(fixtureResult, testSummary);
                        anySpecExecuted = true;
                    }
                }
                if (!anySpecExecuted)
                {
                    testSummary.Record(Result.Exception);
                    Console.WriteLine(string.Format("no active specification found for fixture: {0}", fixture.GetType().FullName));
                }
                return testSummary;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var exceptionResult = new SummarizingResultRecorder();
                exceptionResult.Record(Result.Exception);
                return exceptionResult;
            }
        }

        private void AddToTestResults(IResultSummary singleResult, IResultRecorder resultSummary)
        {
            if (resultSummary == null) return;

            if (singleResult.HasExceptions)
            {
                resultSummary.Record(Result.Exception);
            }
            else if (singleResult.HasFailures)
            {
                resultSummary.Record(Result.Failure);
            }
            else
            {
                resultSummary.Record(Result.Success);
            }
        }
    }
}
