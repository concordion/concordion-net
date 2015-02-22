using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Concordion.NUnit.Addin;
using Concordion.Spec.Concordion.Command;
using Concordion.Spec.Concordion.Command.AssertEquals;
using NUnit.Core;
using NUnit.Framework;

namespace Concordion.Test.Integration.Result
{
    [TestFixture]
    public class NUnitAddinTest
    {
        [Test]
        public void FailureMesssageShouldIncludeResultFile()
        {
            var originalAssertEqualsResult = EvaluatingCommandsTest.assertEqualsResult;
            EvaluatingCommandsTest.assertEqualsResult = "wrong result";
            var assertEqualsTest = new SuiteBuilderAddin().BuildFrom(typeof(EvaluatingCommandsTest));
            var recordingListener = new RecordingListener();
            var testResult = assertEqualsTest.Run(recordingListener, TestFilter.Empty);
            EvaluatingCommandsTest.assertEqualsResult = originalAssertEqualsResult;
            Assert.IsTrue(testResult.Message.Contains("EvaluatingCommands.html"), "NUnit output doesn't contain path to result file");
            Assert.IsTrue(testResult.StackTrace.Contains("concordion:assertequals=\"ForAssertEquals()\" "), "stacktrace doesn't contain snippet");
        }

        [Test]
        public void ErrorMesssageShouldIncludeResultFile()
        {
            EvaluatingCommandsTest.exceptionResult = new Exception("to test error handling");
            var assertEqualsTest = new SuiteBuilderAddin().BuildFrom(typeof(EvaluatingCommandsTest));
            var recordingListener = new RecordingListener();
            var testResult = assertEqualsTest.Run(recordingListener, TestFilter.Empty);
            EvaluatingCommandsTest.exceptionResult = null;
            Assert.IsTrue(testResult.Message.Contains("EvaluatingCommands.html"), "NUnit output doesn't contain path to result file");
        }

        [Test]
        public void ErrorMessageShouldContainException()
        {
            EvaluatingCommandsTest.exceptionResult = new Exception("to test error handling");
            var assertEqualsTest = new SuiteBuilderAddin().BuildFrom(typeof(EvaluatingCommandsTest));
            var recordingListener = new RecordingListener();
            var testResult = assertEqualsTest.Run(recordingListener, TestFilter.Empty);
            EvaluatingCommandsTest.exceptionResult = null;
            Assert.IsTrue(testResult.Message.Contains("MethodFailedException "), "NUnit output doesn't contain exception");
            Assert.IsTrue(testResult.StackTrace.Contains("ExceptionCatchingDecorator.cs:"), "stacktrace does not contain ExceptionCatchingDecorator");
        }

        [Test]
        public void FailingAssertTrueShouldBeVisibleInMessage()
        {
            EvaluatingCommandsTest.assertTrueResult = false;
            var assertEqualsTest = new SuiteBuilderAddin().BuildFrom(typeof(EvaluatingCommandsTest));
            var recordingListener = new RecordingListener();
            var testResult = assertEqualsTest.Run(recordingListener, TestFilter.Empty);
            EvaluatingCommandsTest.assertTrueResult = true;
            Assert.IsTrue(testResult.Message.Contains("expected true but was false"), "NUnit output doesn't contain failure message");
        }

        [Test]
        public void FailingAssertFalseShouldBeVisibleInMessage()
        {
            EvaluatingCommandsTest.assertFalseResult = true;
            var assertEqualsTest = new SuiteBuilderAddin().BuildFrom(typeof(EvaluatingCommandsTest));
            var recordingListener = new RecordingListener();
            var testResult = assertEqualsTest.Run(recordingListener, TestFilter.Empty);
            EvaluatingCommandsTest.assertFalseResult = false;
            Assert.IsTrue(testResult.Message.Contains("expected false but was true"), "NUnit output doesn't contain failure message");
        }

        [Test]
        public void MissingRowsShouldBeVisibleInVerifyRowsMessage()
        {
            var originalVerifyRowsResult = EvaluatingCommandsTest.verifyRowsResult;
            EvaluatingCommandsTest.verifyRowsResult = new[] { "value1"};
            var assertEqualsTest = new SuiteBuilderAddin().BuildFrom(typeof(EvaluatingCommandsTest));
            var recordingListener = new RecordingListener();
            var testResult = assertEqualsTest.Run(recordingListener, TestFilter.Empty);
            EvaluatingCommandsTest.verifyRowsResult = originalVerifyRowsResult;
            Assert.IsTrue(testResult.Message.Contains("missing row"), "NUnit output doesn't contain missing rows message");
        }

        [Test]
        public void WrongRowsShouldBeVisibleInVerifyRowsMessage()
        {
            var originalVerifyRowsResult = EvaluatingCommandsTest.verifyRowsResult;
            EvaluatingCommandsTest.verifyRowsResult = new[] { "value1", "value3" };
            var assertEqualsTest = new SuiteBuilderAddin().BuildFrom(typeof(EvaluatingCommandsTest));
            var recordingListener = new RecordingListener();
            var testResult = assertEqualsTest.Run(recordingListener, TestFilter.Empty);
            EvaluatingCommandsTest.verifyRowsResult = originalVerifyRowsResult;
            Assert.IsTrue(testResult.Message.Contains("expected value2 but was value3"), "NUnit output doesn't contain missing rows message");
        }

        [Test]
        public void FailedRunTestShouldShowFailureInResult()
        {
            var originalGreeting = Greeter.greeting;
            Greeter.greeting = "wrong greeting";
            var assertEqualsTest = new SuiteBuilderAddin().BuildFrom(typeof(EvaluatingCommandsTest));
            var recordingListener = new RecordingListener();
            var testResult = assertEqualsTest.Run(recordingListener, TestFilter.Empty);
            Greeter.greeting = originalGreeting;
            Assert.IsTrue(testResult.Message.Contains("AssertEquals.html failed"), "NUnit output doesn't contain missing rows message");
        }

        [Test]
        public void NUnitGuiRunnerRequiresEventListenerTestFinishedCall()
        {
            var originalGreeting = Greeter.greeting;
            Greeter.greeting = "wrong greeting message";
            var assertEqualsTest = new SuiteBuilderAddin().BuildFrom(typeof(AssertEqualsTest));
            var recordingListener = new RecordingListener();
            assertEqualsTest.Run(recordingListener, TestFilter.Empty);
            Greeter.greeting = originalGreeting;
            Assert.AreEqual(1, recordingListener.testFinished.Count, "wront number of tests executed");
            Assert.AreEqual("Executable Specification: AssertEqualsTest", recordingListener.testFinished[0]);
        }

        [Test]
        public void ResharperRequiresEventListenerTestFinishedCall()
        {
            var originalGreeting = Greeter.greeting;
            Greeter.greeting = "wrong greeting message";
            var assertEqualsTest = new SuiteBuilderAddin().BuildFrom(typeof(AssertEqualsTest));
            var recordingListener = new RecordingListener();
            assertEqualsTest.Run(recordingListener, TestFilter.Empty);
            Greeter.greeting = originalGreeting;
            Assert.AreEqual(1, recordingListener.suiteFinished.Count, "wront number of test suites executed");
            Assert.AreEqual("AssertEqualsTest", recordingListener.suiteFinished[0]);
        }
    }

    [Serializable]
    public class RecordingListener : EventListener
    {
        public ArrayList testStarted = new ArrayList();
        public ArrayList testFinished = new ArrayList();
        public ArrayList suiteStarted = new ArrayList();
        public ArrayList suiteFinished = new ArrayList();

        public TestResult lastResult = null;

        public void RunStarted(string name, int testCount)
        {
        }

        public void RunFinished(TestResult result)
        {
        }

        public void RunFinished(Exception exception)
        {
        }

        public void TestStarted(TestName testName)
        {
            this.testStarted.Add(testName.Name);
        }

        public void TestFinished(TestResult result)
        {
            this.testFinished.Add(result.Name);
            this.lastResult = result;
        }

        public void SuiteStarted(TestName suiteName)
        {
            this.suiteStarted.Add(suiteName.Name);
        }

        public void SuiteFinished(TestResult result)
        {
            this.suiteFinished.Add(result.Name);
        }

        public void UnhandledException(Exception exception)
        {
        }

        public void TestOutput(TestOutput testOutput)
        {
        }
    }
}
