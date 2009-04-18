using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Model.Execution;
using Gallio.Runtime.ProgressMonitoring;
using Gallio.Model;
using Gallio.ConcordionAdapter.Properties;
using System.Reflection;
using Concordion;
using Concordion.Integration;
using Concordion.Internal;

namespace Gallio.ConcordionAdapter.Model
{
    /// <summary>
    /// Controls the execution of Concordion tests
    /// </summary>
    public class ConcordionTestController : BaseTestController
    {
        /// <inheritdoc />
        protected override TestOutcome RunTestsImpl(ITestCommand rootTestCommand, ITestStep parentTestStep, TestExecutionOptions options, IProgressMonitor progressMonitor)
        {
            using (progressMonitor.BeginTask(Resources.ConcordionTestController_RunningConcordionTests, rootTestCommand.TestCount))
            {
                if (options.SkipTestExecution)
                {
                    SkipAll(rootTestCommand, parentTestStep);
                    return TestOutcome.Skipped;
                }
                else
                {
                    bool success = RunTest(rootTestCommand, parentTestStep, progressMonitor);
                    return success ? TestOutcome.Passed : TestOutcome.Failed;
                }
            }
        }

        private static bool RunTest(ITestCommand testCommand, ITestStep parentTestStep, IProgressMonitor progressMonitor)
        {
            ITest test = testCommand.Test;
            progressMonitor.SetStatus(test.Name);

            bool passed;
            ConcordionTest concordionTest = test as ConcordionTest;
            if (concordionTest == null)
            {
                passed = RunChildTests(testCommand, parentTestStep, progressMonitor);
            }
            else
            {
                passed = RunTestFixture(testCommand, concordionTest, parentTestStep);
            }

            progressMonitor.Worked(1);
            return passed;
        }

        private static bool RunTestFixture(ITestCommand testCommand, ConcordionTest concordionTest, ITestStep parentTestStep)
        {
            ITestContext testContext = testCommand.StartPrimaryChildStep(parentTestStep);
            
            // The magic happens here!
            var concordion = new ConcordionBuilder()
                                    .WithSource(concordionTest.Source)
                                    .WithTarget(concordionTest.Target)
                                    .WithSpecificationListener(new GallioResultRenderer(testContext.LogWriter))
                                    .Build();

            var summary = concordion.Process(concordionTest.Resource, concordionTest.Fixture);
            bool passed = !(summary.HasFailures || summary.HasExceptions);
            testContext.AddAssertCount((int)summary.SuccessCount + (int)summary.FailureCount);
            testContext.FinishStep(passed ? TestOutcome.Passed : TestOutcome.Failed, null);
            return passed;
        }

        private static bool RunChildTests(ITestCommand testCommand, ITestStep parentTestStep, IProgressMonitor progressMonitor)
        {
            ITestContext testContext = testCommand.StartPrimaryChildStep(parentTestStep);

            bool passed = true;
            foreach (ITestCommand child in testCommand.Children)
                passed &= RunTest(child, testContext.TestStep, progressMonitor);

            testContext.FinishStep(passed ? TestOutcome.Passed : TestOutcome.Failed, null);
            return passed;
        }
    }
}
