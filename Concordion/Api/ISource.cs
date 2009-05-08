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
    /// Represents the source for specifications
    /// </summary>
    public interface ISource
    {
        /// <summary>
        /// Creates the reader.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        TextReader CreateReader(Resource resource);

        /// <summary>
        /// Determines whether this instance can find the specified resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can find the specified resource; otherwise, <c>false</c>.
        /// </returns>
        bool CanFind(Resource resource);
    }
}
