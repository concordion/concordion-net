using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Concordion.Api
{
    public class Element
    {
        #region Fields
        
        private XElement m_element;

        #endregion

        #region Properties
        
        public string Text
        {
            get
            {
                return m_element.Value;
            }
        }

        public bool HasChildren
        {
            get
            {
                return m_element.HasElements;
            }
        }

        private bool IsBlank
        {
            get
            {
                return String.IsNullOrEmpty(Text.Trim());
            }
        }

        #endregion

        #region Constructors

        public Element(string name)
        {
            m_element = new XElement(name);
        }

        public Element(XElement element)
        {
            m_element = element;
        }

        #endregion

        #region Methods

        public Element AppendText(string text)
        {
            m_element.Add(text);
            return this;
        }

        public void AppendChild(Element child)
        {
            m_element.Add(child.m_element);
        }

        public Element PrependChild(Element child)
        {
            m_element.AddFirst(child.m_element);
            return this;
        }

        public bool IsNamed(string name)
        {
            return m_element.Name == name;
        }

        public IList<Element> GetDescendantElements(string name)
        {
            IList<Element> descendantElements = new List<Element>();
            foreach (XElement element in m_element.Descendants())
            {
                if (element.Name == name)
                {
                    descendantElements.Add(new Element(element));
                }
            }
            return descendantElements;
        }

        public IList<Element> GetChildElements()
        {
            var childElements = new List<Element>();
            foreach (XElement childElement in m_element.Elements())
            {
                childElements.Add(new Element(childElement));
            }
            return childElements;
        }

        public Element GetFirstChildElement(string elementName)
        {
            foreach (XElement descendant in m_element.Descendants())
            {
                if (descendant.Name.LocalName == elementName)
                {
                    return new Element(descendant);
                }
            }

            return null;
        }

        public string GetAttributeValue(string attributeName)
        {
            XAttribute attribute = m_element.Attribute(XName.Get(attributeName));
            return (attribute != null) ? attribute.Value : null;
        }

        public Element AddStyleClass(string style)
        {
            string currentClass = GetAttributeValue("class");
            string styleClass = style;
            if (currentClass != null)
            {
                styleClass = currentClass + " " + styleClass;
            }
            AddAttribute("class", styleClass);
            return this;
        }

        public Element AddAttribute(string localName, string value)
        {
            m_element.SetAttributeValue(XName.Get(localName, ""), value);
            return this;
        }

        public Element GetFirstDescendantNamed(string name)
        {
            foreach (XElement element in m_element.Descendants(XName.Get(name, "")))
            {
                return new Element(element);
            }

            return null;
        }

        public void MoveChildrenTo(Element element)
        {
            foreach (XElement childNode in GetChildNodes()) 
            {
                childNode.Remove();
                element.m_element.Add(childNode);
            }
        }

        private IEnumerable<XElement> GetChildNodes()
        {
            return m_element.Elements();
        }

        public Element AppendNonBreakingSpaceIfBlank()
        {
            if (IsBlank)
            {
                AppendNonBreakingSpace();
            }
            return this;
        }

        public Element AppendNonBreakingSpace()
        {
            return AppendText("\u00A0");
        }

        public Element SetId(string id)
        {
            AddAttribute("id", id);
            return this;
        }

        public Element GetRootElement()
        {
            return new Element(m_element.Document.Root);
        }

        public string ToXml()
        {
            return m_element.ToString();
        }

        public void PrependText(string text)
        {
            m_element.AddFirst(new XText(text));
        }

        #endregion

        #region Override Methods

        public override int GetHashCode()
        {
            return m_element.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Element))
            {
                return false;
            }

            if (obj == null)
            {
                return false;
            }

            Element other = obj as Element;

            if (m_element == null && other.m_element != null)
            {
                return false;
            }
            else if (!m_element.Equals(other))
            {
                return false;
            }

            return true;
        }

        #endregion

    }
}
