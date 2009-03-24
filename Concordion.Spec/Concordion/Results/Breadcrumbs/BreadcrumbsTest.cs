using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Spec.Concordion.Results.Breadcrumbs
{
    class BreadcrumbsTest : AbstractBreadcrumbsTest
    {
        protected override void setUpResource(string resourceName, string content) 
        {
            base.setUpResource(resourceName, content);
        }

        protected override Result getBreadcrumbsFor(string resourceName)
        {
            return base.getBreadcrumbsFor(resourceName);
        }
    }
}
