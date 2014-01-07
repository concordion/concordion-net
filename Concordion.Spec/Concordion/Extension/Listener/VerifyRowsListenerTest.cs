using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Extension.Listener
{
    [ConcordionTest]
    public class VerifyRowsListenerTest : AbstractExtensionTestCase
    {
        public void addLoggingExtension()
        {
            Extension = new LoggingExtension(LogWriter);
        }

        public List<String> getGeorgeAndRingo()
        {
            var result = new string[] {"George Harrison", "Ringo Starr"};
            return result.ToList();
        }
    }
}
