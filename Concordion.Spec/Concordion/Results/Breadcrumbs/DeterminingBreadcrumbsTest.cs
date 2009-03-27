using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Spec.Concordion.Results.Breadcrumbs
{
    class DeterminingBreadcrumbsTest : AbstractBreadcrumbsTest
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
