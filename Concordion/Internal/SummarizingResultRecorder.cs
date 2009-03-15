using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using System.IO;

namespace Concordion.Internal
{
    public class SummarizingResultRecorder : IResultRecorder, IResultSummary
    {
        #region Properties

        private List<Result> RecordedResults
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public SummarizingResultRecorder()
        {
            RecordedResults = new List<Result>();
        }

        #endregion

        #region Methods

        private long GetCount(Result result)
        {
            long count = 0;
            foreach (Result candidate in RecordedResults)
            {
                if (candidate == result) count++;
            }
            return count;
        }

        #endregion

        #region IResultRecorder Members

        public void Record(Result result)
        {
            RecordedResults.Add(result);
        }

        #endregion

        #region IResultSummary Members

        public long SuccessCount
        {
            get { return GetCount(Result.Success); }
        }

        public long FailureCount
        {
            get { return GetCount(Result.Failure); }
        }

        public long ExceptionCount
        {
            get { return GetCount(Result.Exception); }
        }

        public bool HasExceptions
        {
            get
            {
                return ExceptionCount > 0;
            }
        }

        public bool HasFailures
        {
            get
            {
                return FailureCount > 0;
            }
        }

        [Obsolete]
        public void AssertIsSatisfied()
        {
        }
        
        public void AssertIsSatisfied(object fixture)
        {
            if (HasFailures)
            {
                throw new ConcordionAssertionException("Specification has failure(s). See output HTML for details.");
            }

            if (HasExceptions)
            {
                throw new ConcordionAssertionException("Specification has exception(s). See output HTML for details.");
            }
        }

        public void Print(TextWriter writer, object fixture)
        {
            writer.Write("Successes: {0}, Failures: {1}", SuccessCount, FailureCount);
            if (HasExceptions)
            {
                writer.Write(", Exceptions: {0}", ExceptionCount);
            }
            writer.WriteLine();
            writer.Flush();
        }

        #endregion
    }
}
