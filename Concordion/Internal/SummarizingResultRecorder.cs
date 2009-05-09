// Copyright 2009 Jeffrey Cameron
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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

        private IFixtureState DetermineFixtureState(object fixture)
        {
            var attributes = fixture.GetType().GetCustomAttributes(false);
            
            if (attributes.Contains(typeof(UnimplementedAttribute)))
            {
                return new UnimplementedFixtureState();
            }
            else if (attributes.Contains(typeof(ExpectedToFailAttribute)))
            {
                return new ExpectedToFailFixtureState();
            }

            return new ExpectedToPassFixtureState();
        }

        #endregion

        #region IResultRecorder Members

        public void Record(Result result)
        {
            RecordedResults.Add(result);
        }

        #endregion

        #region IResultSummary Members

        /// <summary>
        /// Gets the success count.
        /// </summary>
        /// <value>The success count.</value>
        public long SuccessCount
        {
            get { return GetCount(Result.Success); }
        }

        /// <summary>
        /// Gets the failure count.
        /// </summary>
        /// <value>The failure count.</value>
        public long FailureCount
        {
            get { return GetCount(Result.Failure); }
        }

        /// <summary>
        /// Gets the exception count.
        /// </summary>
        /// <value>The exception count.</value>
        public long ExceptionCount
        {
            get { return GetCount(Result.Exception); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has exceptions.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has exceptions; otherwise, <c>false</c>.
        /// </value>
        public bool HasExceptions
        {
            get
            {
                return ExceptionCount > 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has failures.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has failures; otherwise, <c>false</c>.
        /// </value>
        public bool HasFailures
        {
            get
            {
                return FailureCount > 0;
            }
        }

        /// <summary>
        /// Asserts the is satisfied.
        /// </summary>
        [Obsolete]
        public void AssertIsSatisfied()
        {
        }

        /// <summary>
        /// Asserts the specification is satisfied.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        public void AssertIsSatisfied(object fixture)
        {
            var state = DetermineFixtureState(fixture);
            state.AssertIsSatisfied(this.SuccessCount, this.FailureCount, this.ExceptionCount);
        }

        /// <summary>
        /// Prints the results.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="fixture">The fixture.</param>
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
