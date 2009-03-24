using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Concordion.Spec.Concordion.Command.Results.Stylesheet
{
    class StylesheetTest
    {
        private XElement outputDocument;

        public void processDocument(string html)
        {
            outputDocument = new TestRig()
                .Process(html)
                .GetXDocument()
                .Root;
        }

        public String getRelativePosition(string outer, string target, string sibling)
        {
            var outerElement = outputDocument.Element(outer);

            int targetIndex = indexOfFirstChildWithName(outerElement, target);
            int siblingIndex = indexOfFirstChildWithName(outerElement, sibling);

            return targetIndex > siblingIndex ? "after" : "before";
        }

        private int indexOfFirstChildWithName(XElement element, string name) 
        {
            int index = 0;
            foreach (var e in element.Elements()) 
            {
                if (e.Name.LocalName.Equals(name)) 
                {
                    return index;
                }
                index++;
            }
            throw new Exception("No child <" + name + "> found.");
        }

        public bool elementTextContains(string elementName, string s1, string s2)
        {
            var element = outputDocument.Document.Root.Element(elementName);
            string text = element.Value;
            return text.Contains(s1) && text.Contains(s2);
        }
    }
}
