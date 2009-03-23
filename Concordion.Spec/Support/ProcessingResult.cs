using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Spec
{
    class ProcessingResult
    {
        private readonly IResultSummary resultSummary;
        private readonly EventRecorder eventRecorder;
        private readonly String documentXML;

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

        internal string GetOutputFragmentXML()
        {
            throw new NotImplementedException();
        }
    }
}
