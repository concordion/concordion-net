using System;
using System.Collections.Generic;
using System.Linq;
using Concordion.Integration;
using Concordion.Internal;

namespace Concordion.Spec.Concordion.Configuration
{
    [ConcordionTest]
    public class BaseInputDirectoryTest
    {
        private static bool m_InTestRun = false;

        public bool DirectoryBasedExecuted(string baseInputDirectory)
        {
            if (m_InTestRun) return true;

            m_InTestRun = true;

            var specificationConfig = new SpecificationConfig().Load(this.GetType());
            specificationConfig.BaseInputDirectory = baseInputDirectory;
            var fixtureRunner = new FixtureRunner(specificationConfig);
            var testResult = fixtureRunner.Run(this);

            m_InTestRun = false;

            return !testResult.HasFailures && !testResult.HasExceptions;
        }

        public bool EmbeddedExecuted()
        {
            if (m_InTestRun) return true;

            m_InTestRun = true;

            var specificationConfig = new SpecificationConfig().Load(this.GetType());
            specificationConfig.BaseInputDirectory = null;
            var fixtureRunner = new FixtureRunner(specificationConfig);
            var testResult = fixtureRunner.Run(this);

            m_InTestRun = false;

            return !testResult.HasFailures && !testResult.HasExceptions;
        }
    }
}
