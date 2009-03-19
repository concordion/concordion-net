using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Concordion.Internal;

namespace Concordion.Test
{
    public class StringExtensions_Fixture
    {
        [Fact]
        public void Test_Can_Remove_First_Instance_In_Middle_Of_String_Successfully()
        {
            var str = "ShouldRemoveThis";
            Assert.Equal<string>("ShouldThis", str.RemoveFirst("Remove"));
        }

        [Fact]
        public void Test_Can_Remove_First_Instance_In_Middle_Of_String_If_Multiple_Instances_Present_Successfully()
        {
            var str = "ShouldRemoveRemoveThis";
            Assert.Equal<string>("ShouldRemoveThis", str.RemoveFirst("Remove"));
        }

        [Fact]
        public void Test_Can_Remove_First_Instance_At_Start_Of_String_Successfully()
        {
            var str = "RemoveThis";
            Assert.Equal<string>("This", str.RemoveFirst("Remove"));
        }

        [Fact]
        public void Test_Can_Remove_First_Instance_At_End_Of_String_Successfully()
        {
            var str = "ShouldRemove";
            Assert.Equal<string>("Should", str.RemoveFirst("Remove"));
        }

        [Fact]
        public void Test_Can_Return_Same_String_If_SubString_Not_Found_Successfully()
        {
            var str = "This";
            Assert.Equal<string>("This", str.RemoveFirst("Remove"));
        }

        [Fact]
        public void Test_Can_Return_Empty_String_Successfully()
        {
            var str = String.Empty;
            Assert.Equal<string>(String.Empty, str.RemoveFirst("Remove"));
        }
    }
}
