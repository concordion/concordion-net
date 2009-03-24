using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Spec.Concordion.Results.Breadcrumbs
{
    class DeterminingBreadcrumbsTest : AbstractBreadcrumbsTest
    {
        protected string getBreadcrumbTextFor(string resourceName)
        {
            return base.getBreadcrumbsFor(resourceName).text;
        }

        protected void setUpResource(string resourceName)
        {
            base.setUpResource(resourceName, "<html />");
        }
    }
}
