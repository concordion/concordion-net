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
using Concordion.Internal.Commands;

namespace Concordion.Internal.Renderer
{
    /// <summary>
    /// Handles events coming from the processing of an <see cref="ISpecification"/> object
    /// </summary>
    public interface ISpecificationListener
    {
        /// <summary>
        /// Handles the SpecificationProcessing event that is triggered before the specification is processed
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="Concordion.Internal.Commands.SpecificationEventArgs"/> instance containing the event data.</param>
        void SpecificationProcessingEventHandler(object sender, SpecificationEventArgs eventArgs);

        /// <summary>
        /// Handles the SpecificationProcessed event that is triggered after the specification is processed
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="Concordion.Internal.Commands.SpecificationEventArgs"/> instance containing the event data.</param>
        void SpecificationProcessedEventHandler(object sender, SpecificationEventArgs eventArgs);
    }
}
