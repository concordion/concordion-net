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
            : base("Executable Specification: {0}", fixtureType.Name)
        {
            this.m_FixtureType = fixtureType;
        }

        #region Overrides of Test

        public override TestResult Run(EventListener listener, ITestFilter filter)
        {
            listener.TestStarted(this.TestName);

            Fixture = Reflect.Construct(m_FixtureType);

            var source = new EmbeddedResourceSource(m_FixtureType.Assembly);
            var target = new FileTarget(new SpecificationConfig().Load(m_FixtureType).BaseOutputDirectory);
            var concordion = new ConcordionBuilder().WithSource(source).WithTarget(target).Build();

            var concordionResult = concordion.Process(Fixture);
            var testResult = NUnitTestResult(concordionResult);

            listener.TestFinished(testResult);

            return testResult;
        }

        private TestResult NUnitTestResult(IResultSummary concordionResult)
        {
            var testResult = new TestResult(this);
            testResult.AssertCount = (int) concordionResult.SuccessCount + (int) concordionResult.FailureCount;
            if (!(concordionResult.HasFailures || concordionResult.HasExceptions))
            {
                testResult.Success();
            }
            else if (concordionResult.HasFailures)
            {
                testResult.Failure("Concordion Test Failures: " + concordionResult.FailureCount,
                                   "for stack trace, please see Concordion test reports");
            }
            else if (concordionResult.HasExceptions)
            {
                testResult.Error(new NUnitException("Exception in Concordion test: please see Concordion test reports"));
            }
            return testResult;
        }

        public override string TestType
        {
            get { return "ConcordionTest"; }
        }

        public override sealed object Fixture { get; set; }

        #endregion
    }
}
