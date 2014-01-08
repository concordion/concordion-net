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

using System.Xml.Linq;
using Concordion.Api.Listener;
using Concordion.Internal.Util;

namespace Concordion.Internal.Listener
{
    public class StylesheetEmbedder : IDocumentParsingListener
    {
        #region Properties
        
        private string StylesheetContent
        {
            get;
            set;
        } 

        #endregion

        #region Constructors

        public StylesheetEmbedder(string stylesheetContent)
        {
            this.StylesheetContent = stylesheetContent;
        }

        #endregion

        #region IDocumentParsingListener Members

        public void BeforeParsing(XDocument document)
        {
            XElement html = document.Root;
            XElement head = html.Element("head");
            Check.NotNull(head, "<head> section is missing from document");
            XElement style = new XElement("style");
            style.SetValue(this.StylesheetContent);
            head.AddFirst(style);
        } 

        #endregion
    }
}
