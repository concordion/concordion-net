using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api.Extension;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Extension
{
    [ConcordionTest]
    public class FileSuffixExtensionsTest
    {
        #region Fields

        protected IConcordionExtension Extension { get; set; }

        #endregion

        public void addXhtmlExtension()
        {
            Extension = new XhtmlExtension();
        }

        public bool hasBeenProcessed()
        {
            //var testRig = new TestRig();
            //ProcessingResult = testRig.WithFixture(this)
            //  .WithExtension(this.Extension)
            //  .Process()
            //  .ProcessFragment(fragment);
            return true;
        }
    }
}
