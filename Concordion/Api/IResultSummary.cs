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
using System.IO;

namespace Concordion.Api
{
    /// <summary>
    /// The results of running a Concordion specification
    /// </summary>
    public interface IResultSummary
    {
        /// <summary>
        /// Gets the success count.
        /// </summary>
        /// <value>The success count.</value>
        long SuccessCount { get; }

        /// <summary>
        /// Gets the failure count.
        /// </summary>
        /// <value>The failure count.</value>
        long FailureCount { get; }

        /// <summary>
        /// Gets the exception count.
        /// </summary>
        /// <value>The exception count.</value>
        long ExceptionCount { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has exceptions.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has exceptions; otherwise, <c>false</c>.
        /// </value>
        bool HasExceptions { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has failures.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has failures; otherwise, <c>false</c>.
        /// </value>
        bool HasFailures { get; }

        /// <summary>
        /// Asserts the specification is satisfied.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        void AssertIsSatisfied(object fixture);

        /// <summary>
        /// Prints the results.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="fixture">The fixture.</param>
        void Print(TextWriter writer, object fixture);
    }
}
