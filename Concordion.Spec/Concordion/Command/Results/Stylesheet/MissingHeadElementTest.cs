using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Command.Results.Stylesheet
{
    [ConcordionTest]
    public class MissingHeadElementTest
    {
        public string process(string html)
        {
            var rootElement = new TestRig()
                                    .Process(html)
                                    .GetXDocument()
                                    .Root;
            removeIrrelevantElements(rootElement);
            return rootElement.ToString(SaveOptions.DisableFormatting);
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
            var footer = body.Element("div");
            footer.Remove();
        }
    }
}
