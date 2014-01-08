using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Concordion.Api.Listener;
using Concordion.Internal.Util;

namespace Concordion.Internal.Listener
{
    public class MetadataCreator : IDocumentParsingListener
    {
        #region Methods

        private bool HasContentTypeMetadata(XElement head)
        {
            var metaChildren = head.Elements("meta");
            return metaChildren
                .Select(metaChild => metaChild.Attribute("http-equiv"))
                .Any(httpEquiv => httpEquiv != null && string.Equals("content-type", httpEquiv.Value, StringComparison.OrdinalIgnoreCase));
        }

        private void AddContentTypeMetadata(XElement head)
        {
            var meta = new XElement("meta");
            meta.SetAttributeValue("http-equiv", "content-type");
            meta.SetAttributeValue("content", "text/html; charset=UTF-8");
            head.AddFirst(meta);
        }

        #endregion


        #region IDocumentParsingListener Members

        public void BeforeParsing(XDocument document)
        {
            var html = document.Root;
            var head = html.Element("head");
            Check.NotNull(head, "<head> section is missing from document");
            if (!HasContentTypeMetadata(head))
            {
                AddContentTypeMetadata(head);
            }
        }

        #endregion

    }
}
