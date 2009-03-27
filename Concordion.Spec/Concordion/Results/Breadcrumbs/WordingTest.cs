using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Spec.Concordion.Results.Breadcrumbs
{
    class WordingTest : AbstractBreadcrumbsTest
    {
        public string getBreadcrumbWordingFor(string resourceName, string content) 
        {
            string packageName = "/" + resourceName.Replace(".html", String.Empty) + "/";
            string otherResourceName = "Demo.html";
            setUpResource(packageName + resourceName, content);
            setUpResource(packageName + otherResourceName, "<html />");
            return getBreadcrumbsFor(packageName + otherResourceName).text.Replace(" *> *", "");
        }
    }
}
