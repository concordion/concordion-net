using Concordion.Api.Extension;

namespace Concordion.Spec.Concordion.Extension.FileSuffix
{
    //[ConcordionTest]
    public class FileSuffixExtensionsTest
    {
        #region Fields

        protected IConcordionExtension Extension { get; set; }

        #endregion

        public void addXhtmlExtension()
        {
            this.Extension = new XhtmlExtension();
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
