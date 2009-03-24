using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Internal.Commands;
using Concordion.Api;
using System.Xml.Linq;
using System.IO;

namespace Concordion.Internal.Renderer
{
    public class BreadCrumbRenderer : ISpecificationListener
    {
        #region Properties

        private ISource Source
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public BreadCrumbRenderer(ISource source)
        {
            Source = source;
        }

        #endregion

        #region Methods

        private Element GetDocumentBody(Element rootElement)
        {
            Element body = rootElement.GetFirstDescendantNamed("body");
            if (body == null)
            {
                body = new Element("body");
                rootElement.AppendChild(body);
            }
            return body;
        }

        private void AppendBreadcrumbsTo(Element breadcrumbSpan, Resource documentResource)
        {
            Resource packageResource = documentResource.Parent;

            while (packageResource != null)
            {
                Resource indexPageResource = packageResource.GetRelativeResource(GetIndexPageName(packageResource));
                if (!indexPageResource.Equals(documentResource) && Source.CanFind(indexPageResource))
                {
                    try
                    {
                        PrependBreadcrumb(breadcrumbSpan, CreateBreadcrumbElement(documentResource, indexPageResource));
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Trouble appending a breadcrumb", e);
                    }
                }
                packageResource = packageResource.Parent;
            }

        }

        private void PrependBreadcrumb(Element span, Element breadcrumb)
        {
            if (span.HasChildren)
            {
                span.PrependText(" ");
            }
            span.PrependText(" >");
            span.PrependChild(breadcrumb);
        }

        private Element CreateBreadcrumbElement(Resource documentResource, Resource indexPageResource)
        {
            XDocument document;

            using (var inputStream = Source.CreateReader(indexPageResource))
            {
                document = XDocument.Load(inputStream);
            }

            var breadcrumbWording = GetBreadcrumbWording(new Element(document.Root), indexPageResource);
            var a = new Element("a");
            a.AddAttribute("href", documentResource.GetRelativePath(indexPageResource));
            a.AppendText(breadcrumbWording);
            return a;
        }

        private string GetBreadcrumbWording(Element rootElement, Resource resource)
        {
            var title = rootElement.GetFirstDescendantNamed("title");

            if (title != null && !String.IsNullOrEmpty(title.Text)) 
            {
                return title.Text;
            }

            var headings = rootElement.GetDescendantElements("h1");
            foreach (var h1 in headings) 
            {
                if (h1 != null && !String.IsNullOrEmpty(h1.Text)) 
                {
                    return h1.Text;
                }
            }

            if (resource != null) 
            {
                var heading = resource.Name;
                if (!String.IsNullOrEmpty(heading))
                {
                    heading = StripExtension(heading);
                    heading = Capitalize(heading);
                    heading = DeCamelCase(heading);
                    return heading;
                }
            }

            return "(Up)";
        }

        private string GetIndexPageName(Resource resource)
        {
            return Capitalize(resource.Name) + ".html";
        }

        private static string Capitalize(String s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return String.Empty;
            }

            return s.Substring(0, 1).ToUpper() + s.Substring(1);
        }

        private string StripExtension(string s)
        {
            return s.Replace("\\.[a-z]+", "");
        }

        private static string DeCamelCase(string s)
        {
            return s.Replace("([0-9a-z])([A-Z])", "$1 $2");
        }

        private static bool IsBlank(string s)
        {
            return String.IsNullOrEmpty(s.Replace("[^a-zA-Z0-9]", ""));
        }

        #endregion

        #region ISpecificationRenderer Members

        public void SpecificationProcessingEventHandler(object sender, SpecificationEventArgs eventArgs)
        {
        }

        public void SpecificationProcessedEventHandler(object sender, SpecificationEventArgs eventArgs)
        {
            try
            {
                Element span = new Element("span").AddStyleClass("breadcrumbs");
                AppendBreadcrumbsTo(span, eventArgs.Resource);

                if (span.HasChildren)
                {
                    GetDocumentBody(eventArgs.Element).PrependChild(span);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        #endregion
    }
}
