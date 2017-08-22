using System;
using System.Collections.Generic;
using System.Linq;
using Concordion.Integration;
using Concordion.Internal;
using System.IO;

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
	    
	    Console.WriteLine(baseInputDirectory);
	    Console.WriteLine(this.GetType().Assembly.GetName().Name);

	    //work around for bug of NUnit GUI runner
            /*baseInputDirectory = baseInputDirectory +
                                 Path.DirectorySeparatorChar +
                                 ".." +
                                 Path.DirectorySeparatorChar +
                                 this.GetType().Assembly.GetName().Name;
				 */
            var specificationConfig = new SpecificationConfig().Load(this.GetType());
            specificationConfig.BaseInputDirectory = baseInputDirectory;
            var fixtureRunner = new FixtureRunner(specificationConfig);
            var testResult = fixtureRunner.Run(this);

            m_InTestRun = false;

	    Console.WriteLine(baseInputDirectory);
            foreach (var failureDetail in testResult.FailureDetails) {
                Console.WriteLine(failureDetail.Message);
                Console.WriteLine(failureDetail.StackTrace);
            }
	    
            foreach (var errorDetail in testResult.ErrorDetails)
            {
                Console.WriteLine(errorDetail.Message);
                Console.WriteLine(errorDetail.StackTrace);
                Console.WriteLine(errorDetail.Exception);
            }

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
