using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Internal.Util;
using System.Xml.Linq;

namespace Concordion.Internal.Renderer
{
    public class StylesheetEmbeddingRenderer : IDocumentParsingListener
    {
        #region Properties
        
        private string StylesheetContent
        {
            get;
            set;
        } 

        #endregion

        #region Constructors

        public StylesheetEmbeddingRenderer(string stylesheetContent)
        {
            StylesheetContent = stylesheetContent;
        }

        #endregion

        #region IDocumentParsingListener Members

        public void DocumentParsingEventHandler(object sender, DocumentParsingEventArgs e)
        {
            XElement html = e.Document.Root;
            XElement head = html.Element("head");
            Check.NotNull(head, "<head> section is missing from document");
            XElement style = new XElement("style");
            style.SetValue(StylesheetContent);
            head.AddFirst(style);
        } 

        #endregion
    }
}
