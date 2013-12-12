using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Concordion.Internal;
using NUnit.Core;
using Concordion.Api;

namespace Concordion.Integration.NUnit.Addin
{
    public class ConcordionTest : Test
    {
        private Type m_FixtureType;

        public ConcordionTest(Type fixtureType)
            : base(string.Format("Executable Specification: {0}", fixtureType.Name))
        {
            this.m_FixtureType = fixtureType;
        }
        #region Overrides of Test
        public override TestResult Run(EventListener listener, ITestFilter filter)
        {
            listener.TestStarted(this.TestName);

            Fixture = Reflect.Construct(m_FixtureType);

            var source = new EmbeddedResourceSource(m_FixtureType.Assembly);
            var specificationConfig = new SpecificationConfig().Load(m_FixtureType);
            var target = new FileTarget(specificationConfig.BaseOutputDirectory);
 
			var testResult = new TestResult(this);
            var specExtensions = specificationConfig.SpecificationFileExtensions;
            foreach (var specExtension in specExtensions)
            {
                var specLocator = new ClassNameBasedSpecificationLocator(specExtension);
                var specResource = specLocator.LocateSpecification(Fixture);
                if (source.CanFind(specResource))
                {
                    var concordion = new ConcordionBuilder()
                                            .WithSource(source)
                                            .WithTarget(target)
                                            .WithSpecificationLocator(specLocator)
                                            .Build();
                    var concordionResult = concordion.Process(Fixture);
                    AddToTestResults(concordionResult, testResult);
                }
            }

            listener.TestFinished(testResult);
            if (!testResult.HasResults)
            {
                testResult.Error(new NUnitException(string.Format("no active specification found for fixture: {0}", m_FixtureType.FullName)));
            }
            
            return testResult;
        }

       private TestResult AddToTestResults(IResultSummary concordionResult, TestResult nunitResult)
        {
            if (nunitResult == null)
            {
                nunitResult = new TestResult(this);
            }

            nunitResult.AssertCount += (int)concordionResult.SuccessCount + (int)concordionResult.FailureCount;

            if (!(nunitResult.IsFailure || nunitResult.IsError)
                && !(concordionResult.HasFailures || concordionResult.HasExceptions))
            {
                nunitResult.Success();
            }
            else if (!nunitResult.IsError && concordionResult.HasFailures)
            {
                nunitResult.Failure("Concordion Test Failures: " + concordionResult.FailureCount,
                                   "for stack trace, please see Concordion test reports");
            }
            else if (concordionResult.HasExceptions)
            {
                nunitResult.Error(new NUnitException("Exception in Concordion test: please see Concordion test reports"));
            }

            return nunitResult;
        }

        public override string TestType
        {
            get { return "ConcordionTest"; }
        }

        public override sealed object Fixture { get; set; }

        #endregion
    }
}
