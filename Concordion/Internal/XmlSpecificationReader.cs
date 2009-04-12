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
using System.Xml.Linq;
using System.IO;
using System.Xml;

namespace Concordion.Internal
{
    public class XmlSpecificationReader : ISpecificationReader
    {
        #region Properties
        
        private ISource Source
        {
            get;
            set;
        }

        private DocumentParser DocumentParser
        {
            get;
            set;
        }

        private Uri Location
        {
            get;
            set;
        }

        #endregion

        #region Constructors
        
        public XmlSpecificationReader(ISource source, DocumentParser documentParser)
        {
            Source = source;
            DocumentParser = documentParser;
        }

        #endregion

        #region ISpecificationReader Members

        public ISpecification ReadSpecification(Resource resource)
        {
            XDocument document;

            using (var inputStream = Source.CreateReader(resource))
            {
                document = XDocument.Load(inputStream);
            }

            return DocumentParser.Parse(document, resource);
        }

        #endregion
    }
}
