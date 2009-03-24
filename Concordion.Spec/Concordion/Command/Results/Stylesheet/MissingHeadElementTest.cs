using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Concordion.Spec.Concordion.Command.Results.Stylesheet
{
    class MissingHeadElementTest
    {
        public string process(string html)
        {
            var rootElement = new TestRig()
                                    .Process(html)
                                    .GetXDocument()
                                    .Root;
            removeIrrelevantElements(rootElement);
            return rootElement.ToString();
        }

        private void removeIrrelevantElements(XElement rootElement)
        {
            removeIrrelevantStylesheet(rootElement);
            removeIrrelevantFooter(rootElement);
        }

        private void removeIrrelevantStylesheet(XElement rootElement)
        {
            var head = rootElement.Element("head");
            var style = head.Element("style");
            style.Remove();
        }

        private void removeIrrelevantFooter(XElement rootElement)
        {
            var body = rootElement.Element("body");
            // TODO - need to repair this
            //body.Remove(rootElement.query("//div[@class='footer']").get(0));
        }
    }
}
