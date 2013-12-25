using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Concordion.Api.Listener;
using Concordion.Internal.Util;

namespace Concordion.Internal.Listener
{
    public class JavaScriptEmbedder : IDocumentParsingListener
    {
        #region Fields

        private readonly String m_JavaScript;

        #endregion


        #region Constructors

        public JavaScriptEmbedder(string javaScript)
        {
            this.m_JavaScript = javaScript;
        }

        #endregion


        #region IDocumentParsingListener Members

        public void BeforeParsing(XDocument document)
        {
            XElement html = document.Root;
            XElement head = html.Element("head");
            Check.NotNull(head, "<head> section is missing from document");
            XElement script = new XElement("script");
            script.SetAttributeValue(XName.Get("type"), "text/javascript");
            script.Add(this.m_JavaScript);
            head.AddFirst(script);
        }

        #endregion

    }
}
