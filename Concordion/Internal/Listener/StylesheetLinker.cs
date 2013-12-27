using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Concordion.Api;
using Concordion.Api.Listener;
using Concordion.Internal.Util;

namespace Concordion.Internal.Listener
{
    public class StylesheetLinker : IDocumentParsingListener, ISpecificationProcessingListener
    {
        #region Fields

        private readonly Resource m_StylesheetResource;

        private XElement m_Link;

        #endregion

        #region Constructors

        public StylesheetLinker(Resource stylesheetResource)
        {
            this.m_StylesheetResource = stylesheetResource;
        }

        #endregion

        #region IDocumentParsingListener Memmbers

        public void BeforeParsing(XDocument document)
        {
            var html = document.Root;
            var head = html.Element("head");
            Check.NotNull(head, "<head> section is missing from document");
            m_Link = new XElement("link");
            m_Link.SetAttributeValue(XName.Get("type"), "text/css");
            m_Link.SetAttributeValue(XName.Get("rel"), "stylesheet");
            m_Link.SetValue("");
            head.Add(m_Link);
        }

        #endregion

        #region ISpecificationProcessingListener Members

        public void BeforeProcessingSpecification(SpecificationProcessingEvent processingEvent)
        {
            Resource resource = processingEvent.Resource;
            var javaScriptPath = resource.GetRelativePath(m_StylesheetResource).Replace("\\", "/");
            m_Link.SetAttributeValue(XName.Get("href"), javaScriptPath);
        }

        public void AfterProcessingSpecification(SpecificationProcessingEvent processingEvent)
        {
        }

        #endregion

    }
}
