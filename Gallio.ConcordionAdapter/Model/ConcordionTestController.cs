using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gallio.Runtime.ProgressMonitoring;
using Gallio.Model;
using Gallio.ConcordionAdapter.Properties;
using System.Reflection;
using Concordion;
using Concordion.Integration;
using Concordion.Internal;
using Gallio.Common.Reflection;
using Gallio.Model.Commands;
using Gallio.Model.Helpers;
using Gallio.Model.Tree;
using Gallio.Model.Contexts;



namespace Gallio.ConcordionAdapter.Model
{
    /// <summary>
    /// Controls the execution of Concordion tests
    /// </summary>
    public class ConcordionTestController : TestController
    {
        /// <inheritdoc />
        protected override TestResult RunImpl(ITestCommand rootTestCommand, TestStep parentTestStep, TestExecutionOptions options, IProgressMonitor progressMonitor)
        {
            using (progressMonitor.BeginTask(Resources.ConcordionTestController_RunningConcordionTests, rootTestCommand.TestCount))
            {
                if (progressMonitor.IsCanceled)
                    return new TestResult(TestOutcome.Canceled);

                if (options.SkipTestExecution)
                {
                    return SkipAll(rootTestCommand, parentTestStep);
                }
                else
                {
                    return RunTest(rootTestCommand, parentTestStep, progressMonitor);
                }
            }
        }

        private static TestResult RunTest(ITestCommand testCommand, TestStep parentTestStep, IProgressMonitor progressMonitor)
        {
            Test test = testCommand.Test;
            progressMonitor.SetStatus(test.Name);

            TestResult result;
            ConcordionTest concordionTest = test as ConcordionTest;
            if (concordionTest == null)
            {
                result = RunChildTests(testCommand, parentTestStep, progressMonitor);
            }
            else
            {
                result = RunTestFixture(testCommand, concordionTest, parentTestStep);
            }

            progressMonitor.Worked(1);
            return result;
        }

        private static TestResult RunTestFixture(ITestCommand testCommand, ConcordionTest concordionTest, TestStep parentTestStep)
        {
            ITestContext testContext = testCommand.StartPrimaryChildStep(parentTestStep);
            
            // The magic happens here!
            var concordion = new ConcordionBuilder()
                                    .WithSource(concordionTest.Source)
                                    .WithTarget(concordionTest.Target)
                                    .WithSpecificationListener(new GallioResultRenderer())
                                    .Build();

            var summary = concordion.Process(concordionTest.Resource, concordionTest.Fixture);
            bool passed = !(summary.HasFailures || summary.HasExceptions);
            testContext.AddAssertCount((int)summary.SuccessCount + (int)summary.FailureCount);
            return testContext.FinishStep(passed ? TestOutcome.Passed : TestOutcome.Failed, null);
        }

        private static TestResult RunChildTests(ITestCommand testCommand, TestStep parentTestStep, IProgressMonitor progressMonitor)
        {
            ITestContext testContext = testCommand.StartPrimaryChildStep(parentTestStep);

            bool passed = true;
            foreach (ITestCommand child in testCommand.Children)
                passed &= RunTest(child, testContext.TestStep, progressMonitor).Outcome.Status == TestStatus.Passed;

            return testContext.FinishStep(passed ? TestOutcome.Passed : TestOutcome.Failed, null);
        }
    }
}
