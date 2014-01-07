using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Concordion.Api.Extension;

namespace Concordion.Spec.Concordion.Extension.Listener
{
    public class LoggingExtension : IConcordionExtension
    {
        private readonly AssertLogger m_AssertLogger;

        private readonly ExecuteLogger m_ExecuteLogger;

        private readonly VerifyRowsLogger m_VerifyRowsLogger;

        public LoggingExtension(TextWriter logWriter)
        {
            this.m_AssertLogger = new AssertLogger(logWriter);
            this.m_ExecuteLogger = new ExecuteLogger(logWriter);
            this.m_VerifyRowsLogger = new VerifyRowsLogger(logWriter);
        }

        public void AddTo(IConcordionExtender concordionExtender)
        {
            concordionExtender.WithAssertEqualsListener(m_AssertLogger);
            concordionExtender.WithAssertTrueListener(m_AssertLogger);
            concordionExtender.WithAssertFalseListener(m_AssertLogger);
            concordionExtender.WithExecuteListener(m_ExecuteLogger);
            concordionExtender.WithVerifyRowsListener(m_VerifyRowsLogger);
        }
    }
}
