using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using System.Xml.Linq;

namespace Concordion.Spec
{
    class ProcessingResult
    {
        private readonly IResultSummary resultSummary;
        private readonly EventRecorder eventRecorder;
        private readonly string documentXML;

        public long SuccessCount
        {
            get
            {
                return resultSummary.SuccessCount;
            }
        }

        public long FailureCount
        {
            get
            {
                return resultSummary.FailureCount;
            }
        }

        public long ExceptionCount
        {
            get
            {
                return resultSummary.ExceptionCount;
            }
        }

        public bool HasFailures
        {
            get
            {
                return FailureCount + ExceptionCount != 0;
            }
        }

        public bool IsSuccess
        {
            get
            {
                return !HasFailures;
            }
        }

        public ProcessingResult(IResultSummary resultSummary, EventRecorder eventRecorder, string documentXML) 
        {
            this.resultSummary = resultSummary;
            this.eventRecorder = eventRecorder;
            this.documentXML = documentXML;
        }

        public string SuccessOrFailureInWords()
        {
            return HasFailures ? "FAILURE" : "SUCCESS";
        }

        public XElement GetOutputFragment()
        {
            foreach (var descendant in GetXDocument().Root.Descendants("fragment"))
            {
                return descendant;
            }
            return null;
        }

        public string GetOutputFragmentXML()
        {
            var fragment = GetOutputFragment();
            if (fragment != null)
            {
                var fragmentString = fragment.ToString();
                return fragmentString.Replace("<fragment>", String.Empty).Replace("</fragment>", String.Empty).Replace("\u00A0", "&#160;");
            }

            throw new InvalidOperationException("Cannot find the fragment");
        }

        public XDocument GetXDocument()
        {
            return XDocument.Parse(documentXML);
        }
    }
}
