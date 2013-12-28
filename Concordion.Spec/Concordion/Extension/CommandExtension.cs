using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Api.Extension;
using Concordion.Internal;

namespace Concordion.Spec.Concordion.Extension
{
    public class CommandExtension : IConcordionExtension
    {
        private readonly TextWriter m_LogWriter;

        public CommandExtension(TextWriter logWriter)
        {
            this.m_LogWriter = logWriter;
        }

        public void AddTo(IConcordionExtender concordionExtender)
        {
            concordionExtender.WithCommand("http://myorg.org/my/extension", "log", new LogCommand(m_LogWriter));
        }
    }
}
