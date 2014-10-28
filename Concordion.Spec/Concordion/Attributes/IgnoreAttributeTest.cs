using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Concordion.Integration;
//using Concordion.NUnit.Addin;
//using NUnit.Core;
using NUnit.Framework;

namespace Concordion.Spec.Concordion.Attributes
{
    //To run this test pleas use the following attribute and
    //additionally comment in also the attributes on the class "ExampleIgnoreTest.cs".
    //The necessary reference on NUnit.core prohibits running Concordion.NET tests with TestDriven.NET
    //[ConcordionTest]
    public class IgnoreAttributeTest
    {
        //public bool IsExampleTestIgnored()
        //{
        //    var suiteBuilderAddin = new SuiteBuilderAddin();
        //    var testWithIgnoreAttribute = suiteBuilderAddin.BuildFrom(typeof(ExampleIgnoreTest));
        //    var testResult = testWithIgnoreAttribute.Run(new NullListener(), TestFilter.Empty);
        //    return !testResult.Executed;
        //}
    }
}
