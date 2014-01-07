using System.Collections.Generic;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Extension.Command
{
    [ConcordionTest]
    public class CustomCommandTest : AbstractExtensionTestCase
    {
        public void addCommandExtension()
        {
            this.Extension = new CommandExtension(LogWriter);
        }

        public List<string> getOutput()
        {
            return this.getEventLog();
        }
    }
}
