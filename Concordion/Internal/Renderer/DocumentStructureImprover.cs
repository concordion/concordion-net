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
using System.Xml.Linq;
using Concordion.Api.Listener;
using Concordion.Internal.Util;

namespace Concordion.Internal.Renderer
{
    public class DocumentStructureImprover : IDocumentParsingListener
    {
        #region Methods

        private bool HasHeadSection(XElement html)
        {
            return html.Element(XName.Get("head", "")) != null;
        }

        private void CopyNodesBeforeBodyIntoHead(XElement html, XElement head)
        {
            foreach (XElement child in NodesBeforeBody(html)) 
            {
                child.Remove();
                head.Add(child);
            }
        }

        private IEnumerable<XElement> NodesBeforeBody(XElement html)
        {
            List<XElement> nodes = new List<XElement>();

            foreach (XElement child in html.Elements())
            {
                if (isBodySection(child))
                {
                    break;
                }
                nodes.Add(child);
            }

            return nodes;
        }

        private bool isBodySection(XElement child) 
        {
            return child.Name.LocalName == "body";
        }

        #endregion

        #region IDocumentParsingListener Members

        public void BeforeParsing(XDocument document)
        {
            XElement html = document.Root;
            Check.IsTrue("html".Equals(html.Name.LocalName),
                    "Only <html> documents are supported (<" + html.Name.LocalName + "> is not)");

            if (!HasHeadSection(html))
            {
                XElement head = new XElement("head");
                CopyNodesBeforeBodyIntoHead(html, head);
                html.AddFirst(head);
            }
        }

        #endregion
    }
}
