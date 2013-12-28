using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Extension
{
    [ConcordionTest]
    public class CustomCommandTest : AbstractExtensionTestCase
    {
        public void addCommandExtension()
        {
            Extension = new CommandExtension(LogWriter);
        }

        public List<String> getOutput()
        {
            return GetEventLog();
        }
    }
}
