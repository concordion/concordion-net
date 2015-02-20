using System;
using System.Collections.Generic;
using System.Linq;
using Concordion.Integration;
using NUnit.Core;
using NUnit.Core.Extensibility;

namespace Concordion.NUnit.Addin
{
    [NUnitAddin(Name = "ConcordionNUnitAddin", Description = "Runs Concordion Tests with NUnit", Type = ExtensionType.Core)]
    public class SuiteBuilderAddin : ISuiteBuilder, IAddin
    {
        #region Implementation of ISuiteBuilder

        public bool CanBuildFrom(Type type)
        {
            return Reflect.HasAttribute(type, ConcordionTestAttribute.AttributeIdentifier, false);
        }

        public Test BuildFrom(Type type)
        {
            var testFixture = new ConcordionTestFixture(type);
            testFixture.Add(new ConcordionTest(type));
            NUnitFramework.ApplyCommonAttributes(type, testFixture);
            return testFixture;
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
