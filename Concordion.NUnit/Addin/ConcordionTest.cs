﻿using System;
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
            listener.TestStarted(TestName);
            Fixture = Reflect.Construct(m_FixtureType);
            var result = Translate(new FixtureRunner().Run(Fixture));
            listener.TestFinished(result);
            return result;
        }

        private TestResult Translate(IResultSummary resultSummary)
        {
            var testResult = new TestResult(this);

            if (resultSummary.HasExceptions)
            {
                testResult.Error(new NUnitException("Exception in Concordion test: please see Concordion test reports"));
            } else if (resultSummary.HasFailures)
            {
                testResult.Failure("Concordion Test Failures: " + resultSummary.FailureCount,
                                   "for stack trace, please see Concordion test reports");
            } else
            {
                testResult.Success();
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
