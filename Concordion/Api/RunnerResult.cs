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
    /// A class to hold the result of running an <see cref="IRunner"/> object
    /// </summary>
    public class RunnerResult
    {
        #region Properties
        
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        public Result Result
        {
            get;
            private set;
        } 

        #endregion

        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RunnerResult"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        public RunnerResult(Result result)
        {
            this.Result = result;
        } 

        #endregion
    }
}
