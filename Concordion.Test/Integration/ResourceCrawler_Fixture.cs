using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Concordion.Integration;
using System.IO;

namespace Concordion.Test.Integration
{
    public class ResourceCrawler_Fixture
    {
        public ResourceCrawler_Fixture()
        {
        }

        ~ResourceCrawler_Fixture()
        {
            TearDownDirectories();
        }

        private static void TearDownDirectories()
        {
            Directory.Delete(@"C:\Temp\Demo", true);
        }

        private void SetUpDirectoryStructureWithOnlyHtml()
        {
            Directory.CreateDirectory(@"C:\Temp\Demo");
            using (File.Create(@"C:\Temp\Demo\Demo.html"))
            {
            }
            using (File.Create(@"C:\Temp\Demo\OtherDemo.html"))
            {
            }
        }

        private void SetUpDirectoryStructureWithMixedFileTypes()
        {
            Directory.CreateDirectory(@"C:\Temp\Demo");
            using (File.Create(@"C:\Temp\Demo\Demo.html"))
            {
            }
            using (File.Create(@"C:\Temp\Demo\OtherDemo.html"))
            {
            }
            using (File.Create(@"C:\Temp\Demo\OtherDemo.txt"))
            {
            }
        }

        [Fact]
        public void Can_Enumerate_All_Resources()
        {
            SetUpDirectoryStructureWithOnlyHtml();

            var resources = new List<string>();
            var resourceCrawler = new ResourceCrawler(@"C:\Temp\Demo");

            foreach (string resourcePath in resourceCrawler)
            {
                resources.Add(resourcePath);
            }

            Assert.Equal<int>(2, resources.Count);
            Assert.Contains<string>(@"C:\Temp\Demo\Demo.html", resources);
            Assert.Contains<string>(@"C:\Temp\Demo\OtherDemo.html", resources);

            TearDownDirectories();
        }

        [Fact]
        public void Can_Enumerate_Only_Html_Resources()
        {
            SetUpDirectoryStructureWithMixedFileTypes();

            var resources = new List<string>();
            var resourceCrawler = new ResourceCrawler(@"C:\Temp\Demo");

            foreach (string resourcePath in resourceCrawler)
            {
                resources.Add(resourcePath);
            }

            Assert.Equal<int>(2, resources.Count);
            Assert.Contains<string>(@"C:\Temp\Demo\Demo.html", resources);
            Assert.Contains<string>(@"C:\Temp\Demo\OtherDemo.html", resources);

            TearDownDirectories();
        }
    }
}
