using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using Concordion.Api;
using Concordion.Internal;

namespace Concordion.Test.Integration
{
    [TestFixture]
    public class FileTarget_Fixture
    {
        [Test]
        public void Test_Can_Get_File_Path_Successfully()
        {
            var resource = new Mock<Resource>("blah\\blah.txt");
            resource.Expect(x => x.Path).Returns("blah\\blah.txt");

            var target = new FileTarget(@"c:\temp");

            Assert.AreEqual(@"c:\temp\blah\blah.txt", target.GetTargetPath(resource.Object));
        }
    }
}
