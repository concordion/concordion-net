using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Concordion.Internal;
using NUnit.Core;
using Concordion.Api;

namespace Concordion.NUnit.Addin
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
            var fixtureRunner = new FixtureRunner();
            var concordionResult = fixtureRunner.Run(Fixture);
            var testResult = NUnitTestResult(concordionResult, fixtureRunner.ResultPath);
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

        private TestResult NUnitTestResult(IResultSummary concordionResult, string resultPath)
        {
            var testResult = new TestResult(this);

            if (concordionResult.HasExceptions)
            {
                var errorDetails = concordionResult.ErrorDetails.First();
                testResult.Error(errorDetails.Exception);
                testResult.SetResult(testResult.ResultState, 
                                     resultPath + Environment.NewLine + testResult.Message, 
                                     testResult.StackTrace);
            }
            else if (concordionResult.HasFailures)
            {
                var failureDetails = concordionResult.FailureDetails.First();
                testResult.Failure(resultPath + Environment.NewLine + failureDetails.Message, failureDetails.StackTrace);
            } else
            {
                testResult.Success(resultPath);
            }

            return testResult;
        }

        #endregion
    }
}
