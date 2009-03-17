using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Xunit;

namespace Concordion.Test.Api
{
    public class Resource_Fixture
    {
        [Fact]
        public void Test_If_Resource_Ends_Without_Slash_Can_Tell_You_Its_Parent_Successfully()
        {
            Assert.Equal<string>(@"c:\", new Resource(@"c:\abc").Parent.Path);
        }

        [Fact]
        public void Test_If_Resource_Ends_With_Slash_Can_Tell_You_Its_Parent_Successfully()
        {
            Assert.Equal<string>(@"c:\", new Resource(@"c:\abc\").Parent.Path);
        }

        [Fact]
        public void Test_If_Nested_Resource_Ends_Without_Slash_Can_Tell_You_Its_Parent_Successfully()
        {
            Assert.Equal<string>(@"c:\abc\", new Resource(@"c:\abc\def").Parent.Path);
        }

        [Fact]
        public void Test_If_Nested_Resource_Ends_With_Slash_Can_Tell_You_Its_Parent_Successfully()
        {
            Assert.Equal<string>(@"c:\abc\", new Resource(@"c:\abc\def\").Parent.Path);
        }

        [Fact]
        public void Test_If_Triple_Nested_Resource_Ends_Without_Slash_Can_Tell_You_Its_Parent_Successfully()
        {
            Assert.Equal<string>(@"c:\abc\def\", new Resource(@"c:\abc\def\ghi").Parent.Path);
        }

        [Fact]
        public void Test_If_Triple_Nested_Resource_Ends_With_Slash_Can_Tell_You_Its_Parent_Successfully()
        {
            Assert.Equal<string>(@"c:\abc\def\", new Resource(@"c:\abc\def\ghi\").Parent.Path);
        }

        [Fact]
        public void Test_If_Parent_Of_Root_Is_Null() 
        {
            Assert.Null(new Resource(@"c:\").Parent);
        }

        [Fact]
        public void Test_If_Paths_Point_To_File_And_Are_Identical_Can_Calculate_Relative_Path()
        {
            var from = new Resource(@"c:\spec\x.html");
            var to = new Resource(@"c:\spec\x.html");

            Assert.Equal<string>("x.html", from.GetRelativePath(to));
        }

        [Fact]
        public void Test_If_Paths_Are_Not_Identical_Can_Calculate_Relative_Path()
        {
            var from = new Resource(@"c:\spec");
            var to = new Resource(@"c:\spec\blah");

            Assert.Equal<string>(@"blah", from.GetRelativePath(to));
        }

        [Fact]
        public void Test_If_Paths_Are_Not_Identical_And_End_In_Slashes_Can_Calculate_Relative_Path()
        {
            var from = new Resource(@"c:\a\b\c\");
            var to = new Resource(@"c:\a\b\x\");

            Assert.Equal<string>(@"..\x", from.GetRelativePath(to));
        }

        [Fact]
        public void Test_If_Paths_Are_Weird_And_End_In_Slashes_Can_Calculate_Relative_Path()
        {
            var from = new Resource(@"c:\x\b\c\");
            var to = new Resource(@"c:\a\b\x\");

            Assert.Equal<string>(@"..\..\..\a\b\x", from.GetRelativePath(to));
        }

        [Fact]
        public void Test_If_Paths_Share_Common_Root_And_End_In_Text_File_Can_Calculate_Relative_Path()
        {
            var from = new Resource(@"c:\a\b\c\file.txt");
            var to = new Resource(@"c:\a\x\x\file.txt");

            Assert.Equal<string>(@"..\..\x\x\file.txt", from.GetRelativePath(to));
        }

        [Fact]
        public void Test_If_Path_To_Image_And_Path_To_Html_File_Can_Calculate_Relative_Path()
        {
            var from = new Resource(@"c:\spec\concordion\breadcrumbs\Breadcrumbs.html");
            var to = new Resource(@"c:\image\cocordion-logo.png");

            Assert.Equal<string>(@"..\..\..\image\concordion-logo.png", from.GetRelativePath(to));
        }

        [Fact]
        public void testGivenRelativePathFromOneResourceReturnsOtherResource() 
        {
            //Assert.Equal<string>("/david.html", GetResourceRelativeTo("/blah.html", "david.html"));
            //Assert.Equal<string>("/david.html", GetResourceRelativeTo("/", "david.html"));
            //Assert.Equal<string>("/blah/david.html", GetResourceRelativeTo("/blah/x", "david.html"));
            //Assert.Equal<string>("/blah/x/david.html", GetResourceRelativeTo("/blah/x/y", "david.html"));
            //Assert.Equal<string>("/blah/x/z/david.html", GetResourceRelativeTo("/blah/x/y", "z/david.html"));
            //Assert.Equal<string>("/blah/style.css", GetResourceRelativeTo("/blah/docs/example.html", "../style.css"));
            //Assert.Equal<string>("/style.css", GetResourceRelativeTo("/blah/docs/example.html", "../../style.css"));
            //Assert.Equal<string>("/blah/style.css", GetResourceRelativeTo("/blah/docs/work/example.html", "../../style.css"));
            //Assert.Equal<string>("/blah/docs/style.css", GetResourceRelativeTo("/blah/docs/work/example.html", "../style.css"));
            //Assert.Equal<string>("/style.css", GetResourceRelativeTo("/blah/example.html", "../style.css"));
            //Assert.Equal<string>("/style.css", GetResourceRelativeTo("/blah/", "../style.css"));
            //Assert.Equal<string>("/style.css", GetResourceRelativeTo("/blah", "style.css"));
            //Assert.Equal<string>("/blah/docs/css/style.css", GetResourceRelativeTo("/blah/docs/work/", "../css/style.css"));
        }

        [Fact]
        public void Test_Throws_Exception_If_Relative_Path_Points_Above_Root() 
        {
            var from = new Resource(@"c:\spec\concordion\breadcrumbs\Breadcrumbs.html");
            //Assert.Throws<Exception>(from.GetRelativeResource(@"c:\image\cocordion-logo.png").Path);
        }
    }
}
