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
    public class JavaScriptLinker : IDocumentParsingListener, ISpecificationProcessingListener
    {
        #region Fields

        private readonly Resource m_JavaScriptResource;
        private XElement m_Script;

        #endregion


        #region Constructors

        public JavaScriptLinker(Resource javaScriptResource)
        {
            this.m_JavaScriptResource = javaScriptResource;
        }

        #endregion


        #region IDocumentParsingListener Memmbers

        public void BeforeParsing(XDocument document)
        {
            var html = document.Root;
            var head = html.Element("head");
            Check.NotNull(head, "<head> section is missing from document");
            m_Script = new XElement("script");
            m_Script.SetAttributeValue(XName.Get("type"), "text/javascript");
            m_Script.SetValue("");
            head.Add(m_Script);
        }

        #endregion


        #region ISpecificationProcessingListener Members

        public void BeforeProcessingSpecification(SpecificationProcessingEvent processingEvent)
        {
            Resource resource = processingEvent.Resource;
            m_Script.SetAttributeValue(XName.Get("src"), resource.GetRelativePath(m_JavaScriptResource));
        }

        public void AfterProcessingSpecification(SpecificationProcessingEvent processingEvent)
        {
        }

        #endregion

    }
}
