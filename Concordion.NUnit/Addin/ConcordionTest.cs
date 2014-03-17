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
        #region Fields

        private readonly Type m_FixtureType;

        private readonly MethodInfo[] m_FixtureSetUpMethods;

        private readonly MethodInfo[] m_FixtureTearDownMethods;

        #endregion

        #region Constructors

        public ConcordionTest(Type fixtureType)
            : base(string.Format("Executable Specification: {0}", fixtureType.Name))
        {
            this.m_FixtureType = fixtureType;

            this.m_FixtureSetUpMethods =
                Reflect.GetMethodsWithAttribute(fixtureType, NUnitFramework.FixtureSetUpAttribute, true);
            this.m_FixtureTearDownMethods =
                Reflect.GetMethodsWithAttribute(fixtureType, NUnitFramework.FixtureTearDownAttribute, true);
        }

        #endregion

        #region Overrides of Test

        public override TestResult Run(EventListener listener, ITestFilter filter)
        {
            listener.TestStarted(this.TestName);

            Fixture = Reflect.Construct(m_FixtureType);

            RunFixtureSetUp();

            var source = new EmbeddedResourceSource(m_FixtureType.Assembly);
            var target = new FileTarget(new SpecificationConfig().Load(m_FixtureType).BaseOutputDirectory);
            var concordion = new ConcordionBuilder().WithSource(source).WithTarget(target).Build();

            var concordionResult = concordion.Process(Fixture);
            var testResult = NUnitTestResult(concordionResult);

            RunFixtureTearDown();

            listener.TestFinished(testResult);

            return testResult;
        }

        public override string TestType
        {
            get { return "ConcordionTest"; }
        }

        public override sealed object Fixture { get; set; }

        #endregion

        #region private methods

        private void RunFixtureSetUp()
        {
            if (m_FixtureSetUpMethods != null)
            {
                foreach (MethodInfo setUpMethod in m_FixtureSetUpMethods)
                    Reflect.InvokeMethod(setUpMethod, setUpMethod.IsStatic ? null : Fixture);
            }
        }

        private void RunFixtureTearDown()
        {
            if (m_FixtureTearDownMethods != null)
            {
                foreach (MethodInfo tearDownMethod in m_FixtureTearDownMethods)
                {
                    Reflect.InvokeMethod(tearDownMethod, tearDownMethod.IsStatic ? null : this.Fixture);
                }
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

        #endregion
    }
}
