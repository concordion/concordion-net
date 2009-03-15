using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Concordion.Api;
using System.Reflection;
using Concordion.Integration;

namespace Concordion.Test.Integration
{
    public class FixtureDiscoverer_Fixture
    {
        [Fact]
        public void Can_Discover_Fixture_Internally()
        {
            var resource = new Resource("Demo.html");
            var discoverer = new FixtureDiscoverer();
            discoverer.LoadAssemblies(new List<string> { Assembly.GetExecutingAssembly().Location });
            object fixture = discoverer.GetFixture(resource);

            Assert.NotNull(fixture);
        }
    }
}
