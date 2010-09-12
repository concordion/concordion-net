using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Model;
using Gallio.ConcordionAdapter.Properties;
using Gallio.Runtime.Extensibility;
using Gallio.Runtime.Logging;

namespace Gallio.ConcordionAdapter.Model
{
    /// <summary>
    /// Builds a test object model, based on Concordion attributes using reflection
    /// </summary>
    public class ConcordionTestFramework : BaseTestFramework
    {
       
        /// <inheritdoc />
        sealed public override TestDriverFactory GetTestDriverFactory()
        {
            return CreateTestDriver;
        }

        private static ITestDriver CreateTestDriver(
            IList<ComponentHandle<ITestFramework, TestFrameworkTraits>> testFrameworkHandles,
            TestFrameworkOptions testFrameworkOptions,
            ILogger logger)
        {
            return new ConcordionTestDriver();
        }


    }
}
