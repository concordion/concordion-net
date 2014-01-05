using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Concordion.Api;
using Concordion.Api.Extension;
using Concordion.Api.Listener;

namespace Concordion.Spec.Concordion.Extension.Configuration
{
    public class FakeExtensionBase : IConcordionExtension, IDocumentParsingListener
    {
        public static readonly String FakeExtensionAttrName = "fake.extensions";
        private readonly string m_Text;

        public FakeExtensionBase() {
            this.m_Text = this.GetType().Name;
        }
    
        public FakeExtensionBase(string text) {
            this.m_Text = text;
        }

        public void BeforeParsing(XDocument document)
        {
            var rootElement = new Element(document.Root);
            var existingValue = rootElement.GetAttributeValue(FakeExtensionAttrName);
            var newValue = this.m_Text;
            if (existingValue != null) {
                newValue = existingValue + ", " + newValue;
            }
            rootElement.AddAttribute(FakeExtensionAttrName, newValue);
        }

        public void AddTo(IConcordionExtender concordionExtender)
        {
            concordionExtender.WithDocumentParsingListener(this);
        }
    }
}
