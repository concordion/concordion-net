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

namespace Concordion.Api
{
    /// <summary>
    /// Evaluates OGNL expressions
    /// </summary>
    public interface IEvaluator
    {
        /// <summary>
        /// Gets the variable.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <returns></returns>
        object GetVariable(string variableName);

        /// <summary>
        /// Sets the variable.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="value">The value.</param>
        void SetVariable(string variableName, object value);

        /// <summary>
        /// Evaluates the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        object Evaluate(string expression);

        /// <summary>
        /// Gets the fixture that concordion uses to evaluate expressions against
        /// </summary>
        /// <value>The fixture.</value>
        object Fixture { get; }
    }
}
