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
            listener.TestStarted(TestName);

            Fixture = Reflect.Construct(m_FixtureType);

            RunFixtureSetUp();

            var source = new EmbeddedResourceSource(m_FixtureType.Assembly);
            var specificationConfig = new SpecificationConfig().Load(m_FixtureType);
            var target = new FileTarget(specificationConfig.BaseOutputDirectory);

            var testResult = new TestResult(this);
            var specExtensions = specificationConfig.SpecificationFileExtensions;
            bool anySpecExecuted = false;
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
                    anySpecExecuted = true;
                }
            }
            if (!anySpecExecuted)
            {
                testResult.Error(new NUnitException(string.Format("no active specification found for fixture: {0}", m_FixtureType.FullName)));
            }

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

        #endregion
    }
}
