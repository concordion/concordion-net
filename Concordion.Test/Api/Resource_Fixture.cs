using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using NUnit.Framework;

namespace Concordion.Test.Api
{
    [TestFixture]
    public class Resource_Fixture
    {
        [Test]
        public void Test_If_Resource_Ends_Without_Slash_Can_Tell_You_Its_Parent_Successfully()
        {
            Assert.AreEqual(@"\", new Resource(@"\abc").Parent.Path);
        }

        [Test]
        public void Test_If_Resource_Ends_With_Slash_Can_Tell_You_Its_Parent_Successfully()
        {
            Assert.AreEqual(@"\", new Resource(@"\abc\").Parent.Path);
        }

        [Test]
        public void Test_If_Nested_Resource_Ends_Without_Slash_Can_Tell_You_Its_Parent_Successfully()
        {
            Assert.AreEqual(@"\abc\", new Resource(@"\abc\def").Parent.Path);
        }

        [Test]
        public void Test_If_Nested_Resource_Ends_With_Slash_Can_Tell_You_Its_Parent_Successfully()
        {
            Assert.AreEqual(@"\abc\", new Resource(@"\abc\def\").Parent.Path);
        }

        [Test]
        public void Test_If_Triple_Nested_Resource_Ends_Without_Slash_Can_Tell_You_Its_Parent_Successfully()
        {
            Assert.AreEqual(@"\abc\def\", new Resource(@"\abc\def\ghi").Parent.Path);
        }

        [Test]
        public void Test_If_Triple_Nested_Resource_Ends_With_Slash_Can_Tell_You_Its_Parent_Successfully()
        {
            Assert.AreEqual(@"\abc\def\", new Resource(@"\abc\def\ghi\").Parent.Path);
        }

        [Test]
        public void Test_If_Parent_Of_Root_Is_Null()
        {
            Assert.Null(new Resource(@"\").Parent);
        }

        [Test]
        public void Test_If_Paths_Point_To_File_And_Are_Identical_Can_Calculate_Relative_Path()
        {
            var from = new Resource(@"\spec\x.html");
            var to = new Resource(@"\spec\x.html");

            Assert.AreEqual("x.html", from.GetRelativePath(to));
        }

        [Test]
        public void Test_If_Paths_Are_Not_Identical_Can_Calculate_Relative_Path()
        {
            var from = new Resource(@"\spec\");
            var to = new Resource(@"\spec\blah");

            Assert.AreEqual(@"blah", from.GetRelativePath(to));
        }

        [Test]
        public void Test_If_Paths_Are_Not_Identical_And_End_In_Slashes_Can_Calculate_Relative_Path()
        {
            var from = new Resource(@"\a\b\c\");
            var to = new Resource(@"\a\b\x\");

            Assert.AreEqual(@"..\x\", from.GetRelativePath(to));
        }

        [Test]
        public void Test_If_Paths_Are_Weird_And_End_In_Slashes_Can_Calculate_Relative_Path()
        {
            var from = new Resource(@"\x\b\c\");
            var to = new Resource(@"\a\b\x\");

            Assert.AreEqual(@"..\..\..\a\b\x\", from.GetRelativePath(to));
        }

        [Test]
        public void Test_If_Paths_Share_Common_Root_And_End_In_Text_File_Can_Calculate_Relative_Path()
        {
            var from = new Resource(@"\a\b\c\file.txt");
            var to = new Resource(@"\a\x\x\file.txt");

            Assert.AreEqual(@"..\..\x\x\file.txt", from.GetRelativePath(to));
        }

        [Test]
        public void Test_If_Path_To_Image_And_Path_To_Html_File_Can_Calculate_Relative_Path()
        {
            var from = new Resource(@"\spec\concordion\breadcrumbs\Breadcrumbs.html");
            var to = new Resource(@"\image\concordion-logo.png");

            Assert.AreEqual(@"..\..\..\image\concordion-logo.png", from.GetRelativePath(to));
        }

        [Test]
        public void Test_Can_Get_Relative_Resource_From_Another_Resource_File_Successfully()
        {
            var resourcePath = @"\blah.html";
            var relativePath = @"david.html";

            Assert.AreEqual(@"\david.html", new Resource(resourcePath).GetRelativeResource(relativePath).Path);
        }

        [Test]
        public void Test_Can_Get_Relative_Resource_With_Root_Path_From_Another_Resource_File_Successfully()
        {
            var resourcePath = @"\";
            var relativePath = @"david.html";

            Assert.AreEqual(@"\david.html", new Resource(resourcePath).GetRelativeResource(relativePath).Path);
        }

        [Test]
        public void Test_Can_Get_Relative_Resource_With_Directory_From_Another_Resource_File_Successfully()
        {
            var resourcePath = @"\blah\x";
            var relativePath = @"david.html";

            Assert.AreEqual(@"\blah\david.html", new Resource(resourcePath).GetRelativeResource(relativePath).Path);
        }

        [Test]
        public void Test_Can_Get_Relative_Resource_With_Multiple_Directory_From_Another_Resource_File_Successfully()
        {
            var resourcePath = @"\blah\x\y";
            var relativePath = @"david.html";

            Assert.AreEqual(@"\blah\x\david.html", new Resource(resourcePath).GetRelativeResource(relativePath).Path);
        }

        [Test]
        public void Test_Can_Get_Relative_Resource_With_Multiple_Directory_From_Another_Resource_File_In_A_Directory_Successfully()
        {
            var resourcePath = @"\blah\x\y";
            var relativePath = @"z\david.html";

            Assert.AreEqual(@"\blah\x\z\david.html", new Resource(resourcePath).GetRelativeResource(relativePath).Path);
        }

        [Test]
        public void Test_Can_Get_Relative_Resource_With_Multiple_Directory_From_Another_Resource_File_In_A_SubDirectory_Successfully()
        {
            var resourcePath = @"\blah\docs\example.html";
            var relativePath = @"..\style.css";

            Assert.AreEqual(@"\blah\style.css", new Resource(resourcePath).GetRelativeResource(relativePath).Path);
        }

        [Test]
        public void Test_Can_Get_Relative_Resource_With_Multiple_Directory_And_File_From_Another_Resource_File_In_A_Directory_Successfully()
        {
            var resourcePath = @"\blah\docs\example.html";
            var relativePath = @"..\..\style.css";

            Assert.AreEqual(@"\style.css", new Resource(resourcePath).GetRelativeResource(relativePath).Path);
        }

        [Test]
        public void Test_Can_Get_Relative_Resource_With_Multiple_Directory_And_File_From_Another_Resource_File_In_A_Directory2_Successfully()
        {
            var resourcePath = @"\blah\docs\work\example.html";
            var relativePath = @"..\..\style.css";

            Assert.AreEqual(@"\blah\style.css", new Resource(resourcePath).GetRelativeResource(relativePath).Path);
        }

        [Test]
        public void Test_Can_Get_Relative_Resource_With_Multiple_Directory_And_File_From_Another_Resource_File_In_A_Directory3_Successfully()
        {
            var resourcePath = @"\blah\docs\work\example.html";
            var relativePath = @"..\style.css";

            Assert.AreEqual(@"\blah\docs\style.css", new Resource(resourcePath).GetRelativeResource(relativePath).Path);
        }

        [Test]
        public void Test_Can_Get_Relative_Resource_With_Multiple_Directory_And_File_From_Another_Resource_File_In_A_Directory4_Successfully()
        {
            var resourcePath = @"\blah\example.html";
            var relativePath = @"..\style.css";

            Assert.AreEqual(@"\style.css", new Resource(resourcePath).GetRelativeResource(relativePath).Path);
        }

        [Test]
        public void Test_Can_Get_Relative_Resource_With_Multiple_Directory_And_File_From_Another_Resource_File_In_A_Directory5_Successfully()
        {
            var resourcePath = @"\blah\";
            var relativePath = @"..\style.css";

            Assert.AreEqual(@"\style.css", new Resource(resourcePath).GetRelativeResource(relativePath).Path);
        }

        [Test]
        public void Test_Can_Get_Relative_Resource_With_Multiple_Directory_And_File_From_Another_Resource_File_In_A_Directory6_Successfully()
        {
            var resourcePath = @"\blah";
            var relativePath = @"style.css";

            Assert.AreEqual(@"\style.css", new Resource(resourcePath).GetRelativeResource(relativePath).Path);
        }

        [Test]
        public void Test_Can_Get_Relative_Resource_With_Multiple_Directory_And_File_From_Another_Resource_File_In_A_Directory7_Successfully()
        {
            var resourcePath = @"\blah\docs\work\";
            var relativePath = @"..\css\style.css";

            Assert.AreEqual(@"\blah\docs\css\style.css", new Resource(resourcePath).GetRelativeResource(relativePath).Path);
        }

        [Test]
        public void Test_Throws_Exception_If_Relative_Path_Points_Above_Root()
        {
            var from = new Resource(@"\spec\concordion\breadcrumbs\Breadcrumbs.html");
            Assert.Throws<Exception>(delegate { from.GetRelativeResource(@"..\..\..\..\cocordion-logo.png"); });
        }

        [Test]
        public void Test_Can_Strip_Drive_Letter_Successfully()
        {
            Assert.AreEqual(@"\blah\", new Resource(@"C:\blah\").Path);
        }
    }
}
