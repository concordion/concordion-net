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
using System.Xml;
using System.Xml.Linq;

namespace Concordion.Api
{
    /// <summary>
    /// A wrapper class for an XML element, usually from the specification or the target of the specification
    /// </summary>
    public class Element
    {
        #region Fields
        private static XNamespace XHTML_NS = "http://www.w3.org/1999/xhtml";

        private XElement m_element;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the text value of the element
        /// </summary>
        public string Text
        {
            get
            {
                return m_element.Value;
            }
        }

        /// <summary>
        /// Gets true if the element has children, false otherwise
        /// </summary>
        public bool HasChildren
        {
            get
            {
                return m_element.HasElements;
            }
        }

        /// <summary>
        /// Gets true if the element's text is empty
        /// </summary>
        private bool IsBlank
        {
            get
            {
                return String.IsNullOrEmpty(Text.Trim());
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new object of the <see cref="Element"/> type
        /// </summary>
        /// <param name="name">The name of the new Element</param>
        public Element(string name)
        {
            m_element = new XElement(name);
        }

        /// <summary>
        /// Constructs a new object of the Elem<see cref="Element"/>ent type
        /// </summary>
        /// <param name="element">The <see cref="System.Xml.Linq.XElement"/> to wrap</param>
        public Element(XElement element)
        {
            m_element = element;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Appends some text to the <see cref="Element"/>
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public Element AppendText(string text)
        {
            m_element.Add(text);
            return this;
        }

        /// <summary>
        /// Appends a child <see cref="Element"/> after this one
        /// </summary>
        /// <param name="child"></param>
        public void AppendChild(Element child)
        {
            m_element.Add(child.m_element);
        }

        /// <summary>
        /// Prepends a child <see cref="Element"/> before this one
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public Element PrependChild(Element child)
        {
            m_element.AddFirst(child.m_element);
            return this;
        }

        /// <summary>
        /// Determines if the <see cref="Element"/> has a name like the parameter
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsNamed(string name)
        {
            return m_element.Name.LocalName == name;
        }

        /// <summary>
        /// Gets all of he descendant <see cref="Element"/> objects with a specific name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IList<Element> GetDescendantElements(string name)
        {
            IList<Element> descendantElements = new List<Element>();
            foreach (XElement element in m_element.Descendants())
            {
                if (element.Name.LocalName == name && (element.Name.Namespace=="" || element.Name.Namespace == XHTML_NS))
                {
                    descendantElements.Add(new Element(element));
                }
            }
            return descendantElements;
        }

        /// <summary>
        /// Gets only the immediate child <see cref="Element"/> of the current one
        /// </summary>
        /// <returns>A list of child elements</returns>
        public IEnumerable<Element> GetChildElements()
        {
            var childElements = new List<Element>();
            foreach (XElement childElement in m_element.Elements())
            {
                childElements.Add(new Element(childElement));
            }
            return childElements;
        }

        /// <summary>
        /// Gets the first child <see cref="Element"/> with the following name
        /// The document is searched in DOM document order
        /// </summary>
        /// <param name="elementName"></param>
        /// <returns>an <see cref="Element"/> object if found, null otherwise</returns>
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

        /// <summary>
        /// Gets the value of an attribute of the <see cref="Element"/>
        /// </summary>
        /// <param name="attributeName">The name of the attribute</param>
        /// <returns>A string with the text of the attribute value, null if the attribute does not exist</returns>
        public string GetAttributeValue(string attributeName)
        {
            XAttribute attribute = m_element.Attribute(XName.Get(attributeName));
            return (attribute != null) ? attribute.Value : null;
        }

        /// <summary>
        /// Gets the value of an attribute of the <see cref="Element"/> in the specified namespace
        /// </summary>
        /// <param name="attributeName">The name of the attribute</param>
        /// <param name="namespaceName">The name of the xml namespace</param>
        /// <returns>A string with th etest of the attribute value, null if the attribute does not exist</returns>
        public string GetAttributeValue(string attributeName, string namespaceName)
        {
            XAttribute attribute = m_element.Attribute(XName.Get(attributeName, namespaceName));
            return (attribute != null) ? attribute.Value : null;
        }

        /// <summary>
        /// Applies a css class to the following element
        /// </summary>
        /// <param name="style">The name of the style to apply</param>
        /// <returns>This object with the style class applied</returns>
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

        /// <summary>
        /// Adds an attribute to the element
        /// </summary>
        /// <param name="localName">The name of the attribute</param>
        /// <param name="value">The value of the attribute</param>
        /// <returns>This object with the attribute added</returns>
        public Element AddAttribute(string localName, string value)
        {
            m_element.SetAttributeValue(XName.Get(localName, ""), value);
            return this;
        }

        /// <summary>
        /// Gets the first descendant that matches the name
        /// </summary>
        /// <param name="name">The name to find</param>
        /// <returns>The <see cref="Element"/> if found, null otherwise</returns>
        public Element GetFirstDescendantNamed(string name)
        {
            var elements=GetDescendantElements(name);
            if (elements.Count > 0)
            {
                return elements[0];
            }
            return null;
        }

        /// <summary>
        /// Moves all of the children of this <see cref="Element"/> to another element
        /// </summary>
        /// <param name="element">The destination element</param>
        public void MoveChildrenTo(Element destinationElement)
        {
            destinationElement.m_element.Add(GetChildNodes());
            m_element.RemoveNodes();
        }

        /// <summary>
        /// Gets all child <see cref="System.Xml.Linq.XNode"/>
        /// </summary>
        /// <returns></returns>
        private IEnumerable<XNode> GetChildNodes()
        {
            return m_element.Nodes();
        }

        /// <summary>
        /// If the <see cref="Element"/> has no text then a <![CDATA[&nbsp;]]> element is appended
        /// </summary>
        /// <returns>This object with the NonBreakingSpace appended</returns>
        public Element AppendNonBreakingSpaceIfBlank()
        {
            if (IsBlank)
            {
                AppendNonBreakingSpace();
            }
            return this;
        }

        /// <summary>
        /// Appends a nonbreaking space to the current <see cref="Element"/>
        /// </summary>
        /// <returns></returns>
        public Element AppendNonBreakingSpace()
        {
            return AppendText("\u00A0");
        }

        /// <summary>
        /// Sets the id of the current element
        /// </summary>
        /// <param name="id">The id to set</param>
        /// <returns>This object with the id set</returns>
        public Element SetId(string id)
        {
            AddAttribute("id", id);
            return this;
        }

        /// <summary>
        /// Gets the root element of the <see cref="System.Xml.Linq.XDocument"/> that this <see cref="Element"/> is contained within 
        /// </summary>
        /// <returns>The root <see cref="Element"/> object of the document</returns>
        public Element GetRootElement()
        {
            return new Element(m_element.Document.Root);
        }

        /// <summary>
        /// Outputs the <see cref="Element"/> to a string as xml
        /// </summary>
        /// <returns>A stirng of xml</returns>
        public string ToXml()
        {
            return m_element.ToString();
        }

        /// <summary>
        /// Adds some text to the first of the text of this <see cref="Element>"/>
        /// </summary>
        /// <param name="text"></param>
        public void PrependText(string text)
        {
            m_element.AddFirst(new XText(text));
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Gets a hashcode of the object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return m_element.GetHashCode();
        }

        /// <summary>
        /// Determines if another object equals this one
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
