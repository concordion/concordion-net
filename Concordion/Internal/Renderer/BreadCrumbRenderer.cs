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
                        // TODO - throw an exception here or log it somehow
                    }
                }
                packageResource = packageResource.Parent;
            }

        }

        private void PrependBreadcrumb(Element breadcrumbSpan, object p)
        {
            throw new NotImplementedException();
        }

        private Element CreateBreadcrumbElement(Resource documentResource, Resource indexPageResource)
        {

            XDocument document = XDocument.Load(Source.CreateReader(indexPageResource));

            String breadcrumbWording = GetBreadcrumbWording(new Element(document.Root), indexPageResource);
            Element a = new Element("a");
            a.AddAttribute("href", documentResource.GetRelativePath(indexPageResource));
            a.AppendText(breadcrumbWording);
            return a;
        }

        private string GetBreadcrumbWording(Element element, Resource indexPageResource)
        {
            throw new NotImplementedException();
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
