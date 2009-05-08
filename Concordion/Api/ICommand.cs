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
using Concordion.Internal;

namespace Concordion.Api
{
    /// <summary>
    /// Represents a concordion command from the specification
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Setups the specified command call.
        /// </summary>
        /// <param name="commandCall">The command call.</param>
        /// <param name="evaluator">The evaluator.</param>
        /// <param name="resultRecorder">The result recorder.</param>
        void Setup(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder);

        /// <summary>
        /// Executes the specified command call.
        /// </summary>
        /// <param name="commandCall">The command call.</param>
        /// <param name="evaluator">The evaluator.</param>
        /// <param name="resultRecorder">The result recorder.</param>
        void Execute(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder);

        /// <summary>
        /// Verifies the specified command call.
        /// </summary>
        /// <param name="commandCall">The command call.</param>
        /// <param name="evaluator">The evaluator.</param>
        /// <param name="resultRecorder">The result recorder.</param>
        void Verify(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder);
    }
}
