using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Test.Api
{
    class Resource_Fixture
    {
        public void Can_Tell_You_Its_Parent()
        {
            Resource resource = new Resource(@"C:\temp");

            //Assert.Equal<string>("/", resource.ParentPathOf("/abc"));
            //Assert.Equal<string>("/", parentPathOf("/abc/"));
            //Assert.Equal<string>("/abc/", parentPathOf("/abc/def"));
            //Assert.Equal<string>("/abc/", parentPathOf("/abc/def/"));
            //Assert.Equal<string>("/abc/def/", parentPathOf("/abc/def/ghi"));
        }
    }
}
