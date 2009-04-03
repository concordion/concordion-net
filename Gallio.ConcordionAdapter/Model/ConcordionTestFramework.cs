using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Model;
using Gallio.ConcordionAdapter.Properties;

namespace Gallio.ConcordionAdapter.Model
{
    /// <summary>
    /// Builds a test object model, based on Concordion attributes using reflection
    /// </summary>
    public class ConcordionTestFramework : BaseTestFramework
    {
        private static readonly Guid FRAMEWORK_ID = new Guid("{B4EF2672-1C7B-11DE-A4BE-8E4956D89593}");

        /// <inheritdoc />
        public override Guid Id
        {
            get { return FRAMEWORK_ID; }
        }

        /// <inheritdoc />
        public override string Name
        {
            get { return Resources.ConcordionTestFramework_ConcordionFrameworkName; }
        }

        /// <inheritdoc />
        public override ITestExplorer CreateTestExplorer(TestModel testModel)
        {
            return new ConcordionTestExplorer(testModel);
        }
    }
}
