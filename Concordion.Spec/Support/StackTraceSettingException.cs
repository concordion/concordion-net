using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Concordion.Spec.Support
{
    internal class StackTraceSettingException : Exception
    {
        public List<string> StackTraceElements
        {
            get;
            set;
        }

        public override string StackTrace
        {
            get
            {
                var builder = new StringBuilder();
                foreach (var element in StackTraceElements)
                {
                    builder.AppendLine(element);
                }
                return builder.ToString();
            }

        }

        public StackTraceSettingException()
            : base()
        {
            StackTraceElements = new List<string>();
        }

        public StackTraceSettingException(string message)
            : base(message)
        {
            StackTraceElements = new List<string>();
        }

        public StackTraceSettingException(string message, Exception inner)
            : base(message, inner)
        {
            StackTraceElements = new List<string>();
        }
    }
}
