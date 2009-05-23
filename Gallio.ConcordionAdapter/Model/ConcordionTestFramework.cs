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
        /// <inheritdoc />
        public override void RegisterTestExplorers(IList<ITestExplorer> explorers)
        {
            explorers.Add(new ConcordionTestExplorer());
        }
    }
}
