using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using System.Xml.Linq;
using Concordion.Spec.Support;

namespace Concordion.Spec.Concordion.Results.Breadcrumbs
{
    public abstract class AbstractBreadcrumbsTest
    {
        private TestRig testRig = new TestRig();

        public virtual void setUpResource(string resourceName, string content) 
        {
            testRig.WithResource(new Resource(resourceName), content);
        }

        public virtual Result getBreadcrumbsFor(string resourceName) 
        {
            var spanElements = testRig
                .Process(new Resource(resourceName))
                .GetXDocument()
                .Root
                .Descendants("span");
            
            Result result = new Result();
            foreach (var span in spanElements) 
            {
                if ("breadcrumbs" == span.Attribute("class").Value) 
                {
                    result.html = span.ToString(SaveOptions.DisableFormatting);
                    result.text = span.Value;
                }
            }
            return result;
        }
    
        public class Result 
        {
            public String text = "";
            public String html = "";
        }
    }
}
