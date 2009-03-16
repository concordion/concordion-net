using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Concordion.Internal.Util;

namespace Concordion.Internal.Renderer
{
    public class DocumentStructureImprovementRenderer : IDocumentParsingListener
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

        public void DocumentParsingEventHandler(object sender, DocumentParsingEventArgs e)
        {
            XElement html = e.Document.Root;
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
