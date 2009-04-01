using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Results.Breadcrumbs
{
    [ConcordionTest]
    public class DeterminingBreadcrumbsTest : AbstractBreadcrumbsTest
    {
        public string getBreadcrumbTextFor(string resourceName)
        {
            return base.getBreadcrumbsFor(resourceName).text;
        }

        public void setUpResource(string resourceName)
        {
            base.setUpResource(resourceName, "<html />");
        }
    }
}
