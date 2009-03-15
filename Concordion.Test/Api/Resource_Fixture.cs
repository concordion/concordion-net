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
        public void testCanTellYouItsParent()
        {
            Assert.Equal<string>("/", ParentPathOf("/abc"));
            Assert.Equal<string>("/", ParentPathOf("/abc/"));
            Assert.Equal<string>("/abc/", ParentPathOf("/abc/def"));
            Assert.Equal<string>("/abc/", ParentPathOf("/abc/def/"));
            Assert.Equal<string>("/abc/def/", ParentPathOf("/abc/def/ghi"));
        }

        [Fact]
        public void testReturnsNullForParentOfRoot() 
        {
            Assert.Null(new Resource("/").Parent);
        }

        [Fact]
        public void testCanCalculateRelativePaths()
        {
            Assert.Equal<string>("x.html", RelativePath("/spec/x.html", "/spec/x.html"));
            Assert.Equal<string>("blah", RelativePath("/spec/", "/spec/blah"));
            Assert.Equal<string>("../x/", RelativePath("/a/b/c/", "/a/b/x/"));
            Assert.Equal<string>("../../../a/b/x/", RelativePath("/x/b/c/", "/a/b/x/"));
            Assert.Equal<string>("../../x/x/file.txt", RelativePath("/a/b/c/file.txt", "/a/x/x/file.txt"));
            Assert.Equal<string>("../../../image/concordion-logo.png", RelativePath("/spec/concordion/breadcrumbs/Breadcrumbs.html", "/image/concordion-logo.png"));
        }

        [Fact]
        public void testGivenRelativePathFromOneResourceReturnsOtherResource() 
        {
            Assert.Equal<string>("/david.html", GetResourceRelativeTo("/blah.html", "david.html"));
            Assert.Equal<string>("/david.html", GetResourceRelativeTo("/", "david.html"));
            Assert.Equal<string>("/blah/david.html", GetResourceRelativeTo("/blah/x", "david.html"));
            Assert.Equal<string>("/blah/x/david.html", GetResourceRelativeTo("/blah/x/y", "david.html"));
            Assert.Equal<string>("/blah/x/z/david.html", GetResourceRelativeTo("/blah/x/y", "z/david.html"));
            Assert.Equal<string>("/blah/style.css", GetResourceRelativeTo("/blah/docs/example.html", "../style.css"));
            Assert.Equal<string>("/style.css", GetResourceRelativeTo("/blah/docs/example.html", "../../style.css"));
            Assert.Equal<string>("/blah/style.css", GetResourceRelativeTo("/blah/docs/work/example.html", "../../style.css"));
            Assert.Equal<string>("/blah/docs/style.css", GetResourceRelativeTo("/blah/docs/work/example.html", "../style.css"));
            Assert.Equal<string>("/style.css", GetResourceRelativeTo("/blah/example.html", "../style.css"));
            Assert.Equal<string>("/style.css", GetResourceRelativeTo("/blah/", "../style.css"));
            Assert.Equal<string>("/style.css", GetResourceRelativeTo("/blah", "style.css"));
            Assert.Equal<string>("/blah/docs/css/style.css", GetResourceRelativeTo("/blah/docs/work/", "../css/style.css"));
        }

        [Fact]
        public void testThrowsExceptionIfRelativePathPointsAboveRoot() 
        {
            try 
            {
                GetResourceRelativeTo("/blah/docs/example.html", "../../../style.css");
                Assert.True(false); // always fail
            } 
            catch (Exception e) 
            {
                Assert.Equal<string>("Path '../../../style.css' relative to '/blah/docs/example.html' " +
                        "evaluates above the root package.", e.Message);
            }
        }

        private string GetResourceRelativeTo(string resourcePath, string relativePath) 
        {
            return new Resource(resourcePath).GetRelativeResource(relativePath).Path;
        }

        private string RelativePath(string from, string to) 
        {
            return resource(from).GetRelativePath(resource(to));
        }

        private string ParentPathOf(string path) 
        {
            return resource(path).Parent.Path;
        }

        private Resource resource(string path) 
        {
            return new Resource(path);
        }
    }
}
