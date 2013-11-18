using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Core;
using NUnit.Core.Extensibility;

namespace Concordion.Integration.NUnit.Addin
{
    [NUnitAddin(Name = "Concordion Test Runner", Description = "Runs Concordion Tests with NUnit", Type = ExtensionType.Core)]
    public class SuiteBuilderAddin : ISuiteBuilder, IAddin
    {
        #region Implementation of ISuiteBuilder

        public bool CanBuildFrom(Type type)
        {
            return Reflect.HasAttribute(type, "Concordion.Integration.ConcordionTestAttribute", false);
        }

        public Test BuildFrom(Type type)
        {
            var testSuite = new TestSuite(type);
            testSuite.Add(new ConcordionTest(type));
            return testSuite;
        }

        #endregion

        #region Implementation of IAddin

        public bool Install(IExtensionHost host)
        {
            IExtensionPoint extensionPoint = host.GetExtensionPoint("SuiteBuilders");
            if (extensionPoint == null) return false;
            extensionPoint.Install(this);
            return true;
        }

        #endregion
    }
}
