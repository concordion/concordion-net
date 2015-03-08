using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Concordion.Internal;

namespace Concordion.Test
{
    [TestFixture]
    public class StringExtensions_Fixture
    {
        [Test]
        public void Test_Can_Remove_First_Instance_In_Middle_Of_String_Successfully()
        {
            var str = "ShouldRemoveThis";
            Assert.AreEqual("ShouldThis", str.RemoveFirst("Remove"));
        }

        [Test]
        public void Test_Can_Remove_First_Instance_In_Middle_Of_String_If_Multiple_Instances_Present_Successfully()
        {
            var str = "ShouldRemoveRemoveThis";
            Assert.AreEqual("ShouldRemoveThis", str.RemoveFirst("Remove"));
        }

        [Test]
        public void Test_Can_Remove_First_Instance_At_Start_Of_String_Successfully()
        {
            var str = "RemoveThis";
            Assert.AreEqual("This", str.RemoveFirst("Remove"));
        }

        [Test]
        public void Test_Can_Remove_First_Instance_At_End_Of_String_Successfully()
        {
            var str = "ShouldRemove";
            Assert.AreEqual("Should", str.RemoveFirst("Remove"));
        }

        [Test]
        public void Test_Can_Return_Same_String_If_SubString_Not_Found_Successfully()
        {
            var str = "This";
            Assert.AreEqual("This", str.RemoveFirst("Remove"));
        }

        [Test]
        public void Test_Can_Return_Empty_String_Successfully()
        {
            var str = String.Empty;
            Assert.AreEqual(String.Empty, str.RemoveFirst("Remove"));
        }
    }
}
