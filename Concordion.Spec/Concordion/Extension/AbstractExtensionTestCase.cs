using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Api.Extension;

namespace Concordion.Spec.Concordion.Extension
{
    public class AbstractExtensionTestCase
    {
        #region Fields

        protected IConcordionExtension Extension { get; set; }

        protected TestRig TestRig { get; set; }

        protected ProcessingResult ProcessingResult { get; set; }

        public TextWriter LogWriter { get; set; }

        #endregion

        public AbstractExtensionTestCase()
        {
            this.LogWriter = new StringWriter();
        }

        public void processAnything()
        {
            process("<p>anything..</p>");
        }
    
        public void process(String fragment)
        {
            TestRig = new TestRig();
            this.ConfigureTestRig();
            ProcessingResult = TestRig.WithFixture(this)
              .WithExtension(this.Extension)
              .ProcessFragment(fragment);
        }

        protected virtual void ConfigureTestRig()
        {
        }

        public List<string> GetEventLog()
        {
            LogWriter.Flush();
            var loggedEvents = LogWriter.ToString().Split(new[] {LogWriter.NewLine}, StringSplitOptions.None);
            var eventLog = loggedEvents.ToList();
            eventLog.Remove("");
            return eventLog;
        }

        public bool isAvailable(string resourcePath) {
            return TestRig.HasCopiedResource(new global::Concordion.Api.Resource(resourcePath));
        }
    }
}
