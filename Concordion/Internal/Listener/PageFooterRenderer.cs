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
using Concordion.Api;
using Concordion.Api.Listener;

namespace Concordion.Internal.Listener
{
    public class PageFooterRenderer : ISpecificationProcessingListener
    {
        #region Fields
        
        private static readonly string CONCORDION_WEBSITE_URL = "http://www.concordion.org";
        private static readonly Resource TARGET_LOGO_RESOURCE = new Resource("image/concordion-logo.png");
        private DateTime start; 

        #endregion

        #region Properties

        private ITarget Target
        {
            get;
            set;
        }
        
        #endregion

        #region Constructors

        public PageFooterRenderer(ITarget target)
        {
            this.Target = target;
        }

        #endregion

        #region Methods

        private void CopyLogoToTarget()
        {
            this.Target.Write(TARGET_LOGO_RESOURCE, HtmlFramework.SOURCE_LOGO_RESOURCE_PATH);
        }

        private void AddFooterToDocument(Element rootElement, Resource resource, long timeTaken)
        {
            Element body = rootElement.GetFirstChildElement("body");

            if (body != null)
            {

                Element footer = new Element("div");
                footer.AddStyleClass("footer");
                footer.AppendText("Powered by ");

                Element link = new Element("a");
                link.AddAttribute("href", CONCORDION_WEBSITE_URL);
                footer.AppendChild(link);

                Element img = new Element("img");
                img.AddAttribute("src", resource.GetRelativePath(TARGET_LOGO_RESOURCE));
                img.AddAttribute("alt", "Concordion");
                img.AddAttribute("border", "0");
                link.AppendChild(img);

                body.AppendChild(footer);
            }
        }

        #endregion

        #region ISpecificationProcessingListener Members

        public void BeforeProcessingSpecification(SpecificationProcessingEvent processingEvent)
        {
            this.start = DateTime.Now;
        }

        public void AfterProcessingSpecification(SpecificationProcessingEvent processingEvent)
        {
            try
            {
                this.CopyLogoToTarget();
                TimeSpan span = new TimeSpan(DateTime.Now.Ticks).Subtract(new TimeSpan(this.start.Ticks));
		this.AddFooterToDocument(processingEvent.RootElement, processingEvent.Resource, span.Ticks);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to write page footer. {0}", e.Message);
            }
        }

        #endregion
    }
}
