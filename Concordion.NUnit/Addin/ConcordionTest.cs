using System;
using System.Collections.Generic;
using System.Linq;
using Concordion.Internal;
using NUnit.Core;
using Concordion.Api;

namespace Concordion.NUnit.Addin
{
    public class ConcordionTest : Test
    {
        private Type m_FixtureType;

        public ConcordionTest(Type fixtureType)
        //    : base(string.Format("Executable Specification: {0}", fixtureType.Name))
            : base(fixtureType.FullName)
        {
            this.m_FixtureType = fixtureType;

            this.fixtureSetUpMethods =
                Reflect.GetMethodsWithAttribute(fixtureType, NUnitFramework.FixtureSetUpAttribute, true);
            this.fixtureTearDownMethods =
                Reflect.GetMethodsWithAttribute(fixtureType, NUnitFramework.FixtureTearDownAttribute, true);
        }

        #region Overrides of Test

        public override TestResult Run(EventListener listener, ITestFilter filter)
        {
            listener.TestStarted(TestName);

            RunSetup();

            var source = new EmbeddedResourceSource(m_FixtureType.Assembly);
            var target = new FileTarget(new SpecificationConfig().Load(m_FixtureType).BaseOutputDirectory);
            var concordion = new ConcordionBuilder().WithSource(source).WithTarget(target).Build();

            Fixture = Reflect.Construct(m_FixtureType);
            var concordionResult = concordion.Process(Fixture);
            var testResult = NUnitTestResult(concordionResult);

            RunTeardown();

            listener.TestFinished(testResult);

            return testResult;
        }

        private void RunFixtureSetUp()
        {
            if (setUpMethods != null)
                foreach (MethodInfo setUpMethod in setUpMethods)
                    Reflect.InvokeMethod(setUpMethod, setUpMethod.IsStatic ? null : this.Fixture);
        }

        private void RunFixtureTearDown(TestResult testResult)
        {
            try
            {
                if (tearDownMethods != null)
                {
                    int index = tearDownMethods.Length;
                    while (--index >= 0)
                        Reflect.InvokeMethod(tearDownMethods[index], tearDownMethods[index].IsStatic ? null : this.Fixture);
                }
            }
            catch (Exception ex)
            {
                if (ex is NUnitException)
                    ex = ex.InnerException;

                RecordException(ex, testResult, FailureSite.TearDown);
            }
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
