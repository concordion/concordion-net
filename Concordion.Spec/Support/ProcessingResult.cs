using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using System.Xml.Linq;
using Concordion.Api.Listener;

namespace Concordion.Spec.Support
{
    public class ProcessingResult
    {
        private readonly IResultSummary resultSummary;
        private readonly EventRecorder eventRecorder;
        private readonly string documentXML;

        public long SuccessCount
        {
            get
            {
                return this.resultSummary.SuccessCount;
            }
        }

        public long FailureCount
        {
            get
            {
                return this.resultSummary.FailureCount;
            }
        }

        public long ExceptionCount
        {
            get
            {
                return this.resultSummary.ExceptionCount;
            }
        }

        public bool HasFailures
        {
            get
            {
                return this.FailureCount + this.ExceptionCount != 0;
            }
        }

        public bool IsSuccess
        {
            get
            {
                return !this.HasFailures;
            }
        }

        public ProcessingResult(IResultSummary resultSummary, EventRecorder eventRecorder, string documentXML) 
        {
            this.resultSummary = resultSummary;
            this.eventRecorder = eventRecorder;
            this.documentXML = documentXML;
        }

        public string SuccessOrFailureInWords()
        {
            return this.HasFailures ? "FAILURE" : "SUCCESS";
        }

        public XElement GetOutputFragment()
        {
            foreach (var descendant in this.GetXDocument().Root.Descendants("fragment"))
            {
                return descendant;
            }
            return null;
        }

        public string GetOutputFragmentXML()
        {
            var fragment = this.GetOutputFragment();
            var xmlFragmentBuilder = new StringBuilder();
            foreach (var child in fragment.Elements())
            {
                //xmlFragmentBuilder.Append(child.ToString(SaveOptions.DisableFormatting).Replace(" xmlns:concordion=\"http://www.concordion.org/2007/concordion\"", String.Empty));
                xmlFragmentBuilder.Append(child.ToString().Replace(" xmlns:concordion=\"http://www.concordion.org/2007/concordion\"", String.Empty));
            }

            return xmlFragmentBuilder.ToString();
        }

        public XDocument GetXDocument()
        {
            return XDocument.Parse(this.documentXML);
        }

        public AssertFailureEvent GetLastAssertEqualsFailureEvent()
        {
            return this.eventRecorder.GetLast(typeof(AssertFailureEvent)) as AssertFailureEvent;
        }

        public Element GetRootElement()
        {
            return new Element(this.GetXDocument().Root);
        }

        public bool HasCssDeclaration(string cssFilename)
        {
            var head = this.GetRootElement().GetFirstChildElement("head");
            return head.GetChildElements("link").Any(
                link =>
                    string.Equals("text/css", link.GetAttributeValue("type")) &&
                    string.Equals("stylesheet", link.GetAttributeValue("rel")) &&
                    string.Equals(cssFilename, link.GetAttributeValue("href")));
        }

        public bool HasEmbeddedCss(string css)
        {
            var head = this.GetRootElement().GetFirstChildElement("head");
            return head.GetChildElements("style").Any(style => style.Text.Contains(css));
        }

        public bool HasJavaScriptDeclaration(string cssFilename) {
            var head = this.GetRootElement().GetFirstChildElement("head");
            return head.GetChildElements("script").Any(
                script => 
                    string.Equals("text/javascript", script.GetAttributeValue("type")) && 
                    string.Equals(cssFilename, script.GetAttributeValue("src")));
        }

        public bool HasEmbeddedJavaScript(string javaScript) {
            var head = this.GetRootElement().GetFirstChildElement("head");
            return head.GetChildElements("script").Any(
                script => 
                    string.Equals("text/javascript", (string) script.GetAttributeValue("type")) && 
                    script.Text.Contains(javaScript));
        }
    }
}
