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
using Concordion.Internal.Commands;

namespace Concordion.Internal.Renderer
{
    public class SpecificationRenderer : ISpecificationListener
    {
        #region Fields
		        
        private static readonly string XML_DECLARATION = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

	    #endregion

        #region Properties

        private ITarget Target
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public SpecificationRenderer(ITarget target)
        {
            Target = target;
        }

        #endregion

        #region ISpecificationRenderer Members

        public void SpecificationProcessingEventHandler(object sender, SpecificationEventArgs eventArgs)
        {
        }

        public void SpecificationProcessedEventHandler(object sender, SpecificationEventArgs eventArgs)
        {
            try 
            {
                Target.Write(eventArgs.Resource, XML_DECLARATION + eventArgs.Element.ToXml());
                if (Target is FileTarget) 
                {
                    // TODO - Replace this with something meaningful

                    Console.WriteLine("Processed specifications : " + ((FileTarget)Target).GetTargetPath(eventArgs.Resource));
                }
            } 
            catch (Exception e) 
            {
                throw new Exception("Failed to write results to '" + eventArgs.Resource.Path + "'.", e);
            }
        }

        #endregion
    }
}
