using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Concordion.Integration;
using Concordion.Spec.Support;

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
            RemoveIrrelevantElements(rootElement);
            return rootElement.ToString(SaveOptions.DisableFormatting);
        }

        private void RemoveIrrelevantElements(XElement rootElement)
        {
            RemoveIrrelevantStylesheet(rootElement);
            RemoveIrrelevantMetadata(rootElement);
            RemoveIrrelevantFooter(rootElement);
        }

        private void RemoveIrrelevantStylesheet(XElement rootElement)
        {
            var head = rootElement.Element("head");
            var style = head.Element("style");
            style.Remove();
        }

        private void RemoveIrrelevantMetadata(XElement rootElement)
        {
            var head = rootElement.Element("head");
            var meta = head.Element("meta");
            meta.Remove();
        }

        private void RemoveIrrelevantFooter(XElement rootElement)
        {
            var body = rootElement.Element("body");
            var footer = body.Element("div");
            footer.Remove();
        }
    }
}
