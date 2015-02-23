using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Core;

namespace Concordion.NUnit.Addin
{
    public class ConcordionTestFixture : TestFixture
    {
        public ConcordionTestFixture(Type fixtureType)
            : base(fixtureType) { }

        public override TestResult Run(EventListener listener, ITestFilter filter)
        {
            listener.SuiteStarted(this.TestName);

            var suiteListener = new MuteSuiteListener(listener);

            var fixtureResult = base.Run(suiteListener, TestFilter.Empty);
            var testResult = fixtureResult.Results[0] as TestResult;

            if (testResult != null)
            {
                if (testResult.IsFailure || testResult.IsError)
                {
                    fixtureResult.SetResult(testResult.ResultState,
                                            testResult.Message,
                                            testResult.StackTrace);
                }
                if (testResult.IsSuccess)
                {
                    fixtureResult.Success();
                }
            }

            listener.SuiteFinished(fixtureResult);
            return fixtureResult;
        }

        protected override void DoOneTimeSetUp(TestResult suiteResult) { }

        protected override void DoOneTimeTearDown(TestResult suiteResult) { }
    }

    [Serializable]
    class MuteSuiteListener : EventListener
    {
        private readonly EventListener m_EventListener;

        public MuteSuiteListener(EventListener eventListener)
        {
            this.m_EventListener = eventListener;
        }

        public void SuiteStarted(TestName testName)
        {
            //do not propagate suite events - necessary for test reporting in resharper
        }

        public void SuiteFinished(TestResult result)
        {
            //do not propagate suite events - necessary for test reporting in resharper
        }

        public void RunStarted(string name, int testCount)
        {
            if (m_EventListener != null)
            {
                m_EventListener.RunStarted(name, testCount);
            }
        }

        public void RunFinished(TestResult result)
        {
            if (m_EventListener != null)
            {
                m_EventListener.RunFinished(result);
            }
        }

        public void RunFinished(Exception exception)
        {
            if (m_EventListener != null)
            {
                m_EventListener.RunFinished(exception);
            }
        }

        public void TestStarted(TestName testName)
        {
            if (m_EventListener != null)
            {
                m_EventListener.TestStarted(testName);
            }
        }

        public void TestFinished(TestResult result)
        {
            if (m_EventListener != null)
            {
                m_EventListener.TestFinished(result);
            }
        }

        public void UnhandledException(Exception exception)
        {
            if (m_EventListener != null)
            {
                m_EventListener.UnhandledException(exception);
            }
        }

        public void TestOutput(TestOutput testOutput)
        {
            if (m_EventListener != null)
            {
                m_EventListener.TestOutput(testOutput);
            }
        }
    }
}
