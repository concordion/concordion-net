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
            return GetXDocument().Root.Element("fragment");
        }

        public string GetOutputFragmentXML()
        {
            return GetOutputFragment().ToString().Replace("</?fragment>", "").Replace("\u00A0", "&#160;");
        }

        public XDocument GetXDocument()
        {
            return XDocument.Parse(documentXML);
        }
    }
}
