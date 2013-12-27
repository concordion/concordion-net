using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using Concordion.Api.Listener;
using Concordion.Internal.Commands;

namespace Concordion.Spec
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
                return resultSummary.SuccessCount;
            }
        }

        public long FailureCount
        {
            get
            {
                return resultSummary.FailureCount;
            }
        }

        public long ExceptionCount
        {
            get
            {
                return resultSummary.ExceptionCount;
            }
        }

        public bool HasFailures
        {
            get
            {
                return FailureCount + ExceptionCount != 0;
            }
        }

        public bool IsSuccess
        {
            get
            {
                return !HasFailures;
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
            return HasFailures ? "FAILURE" : "SUCCESS";
        }

        public XElement GetOutputFragment()
        {
            foreach (var descendant in GetXDocument().Root.Descendants("fragment"))
            {
                return descendant;
            }
            return null;
        }

        public string GetOutputFragmentXML()
        {
            var fragment = GetOutputFragment();
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
            return XDocument.Parse(documentXML);
        }

        public AssertFailureEvent GetLastAssertEqualsFailureEvent()
        {
            return eventRecorder.GetLast(typeof(AssertFailureEvent)) as AssertFailureEvent;
        }

        public Element GetRootElement()
        {
            return new Element(GetXDocument().Root);
        }

        public bool HasCssDeclaration(string cssFilename)
        {
            var head = GetRootElement().GetFirstChildElement("head");
            return head.GetChildElements("link").Any(
                link =>
                    string.Equals("text/css", link.GetAttributeValue("type")) &&
                    string.Equals("stylesheet", link.GetAttributeValue("rel")) &&
                    string.Equals(cssFilename, link.GetAttributeValue("href")));
        }

        public bool HasEmbeddedCss(string css)
        {
            var head = GetRootElement().GetFirstChildElement("head");
            return head.GetChildElements("style").Any(style => style.Text.Contains(css));
        }

        public bool HasJavaScriptDeclaration(string cssFilename) {
            var head = GetRootElement().GetFirstChildElement("head");
            return head.GetChildElements("script").Any(
                script => 
                    string.Equals("text/javascript", script.GetAttributeValue("type")) && 
                    string.Equals(cssFilename, script.GetAttributeValue("src")));
        }

        public bool HasEmbeddedJavaScript(string javaScript) {
            var head = GetRootElement().GetFirstChildElement("head");
            return head.GetChildElements("script").Any(
                script => 
                    string.Equals("text/javascript", (string) script.GetAttributeValue("type")) && 
                    script.Text.Contains(javaScript));
        }
    }
}
