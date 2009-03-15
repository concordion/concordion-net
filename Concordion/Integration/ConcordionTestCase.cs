using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Internal;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Concordion.Integration
{
    public class ConcordionTestCase : IConcordionTestCase
    {
        public ConcordionTestCase()  
        {
            string resourcePath = Directory.GetCurrentDirectory();
            foreach (Attribute attribute in this.GetType().GetCustomAttributes(true))
            {
                HtmlResourceAttribute htmlResourceAttribute = attribute as HtmlResourceAttribute;

                if (htmlResourceAttribute != null)
                {
                    resourcePath = htmlResourceAttribute.Path;
                    break;
                }
            }

            Resource resource = new Resource(resourcePath);
            IResultSummary resultSummary = new ConcordionBuilder().Build().Process(resource, this);
            resultSummary.Print(System.Console.Out, this);
            resultSummary.AssertIsSatisfied(this);
        }
    }
}
