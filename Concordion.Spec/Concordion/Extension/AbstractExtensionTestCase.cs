using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Api.Extension;

namespace Concordion.Spec.Concordion.Extension
{
    public class AbstractExtensionTestCase
    {
        private List<String> eventList;
        private TestRig testRig;
        private ProcessingResult processingResult;
        private IConcordionExtension extension;

        public void processAnything()
        {
            process("<p>anything..</p>");
        }
    
        public void process(String fragment)
        {
            testRig = new TestRig();
            configureTestRig(testRig);
            processingResult = testRig.WithFixture(this)
              .WithExtension(extension)
              .ProcessFragment(fragment);
        }

        protected virtual void configureTestRig(TestRig testRig)
        {
        }

        public bool isAvailable(string resourcePath) {
            return testRig.HasCopiedResource(new Resource(resourcePath));
        }

        protected ProcessingResult getProcessingResult() {
            return processingResult;
        }
    
        protected void SetExtension(IConcordionExtension extension) {
            this.extension = extension;
        }
    }
}
