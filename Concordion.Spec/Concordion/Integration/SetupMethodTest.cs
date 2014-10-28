using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Integration;
using NUnit.Framework;

namespace Concordion.Spec.Concordion.Integration
{
    [ConcordionTest]
    public class SetupMethodTest
    {
        private List<string> m_CalledMethods;
        private List<string> CalledMethods
        {
            get
            {
                if (m_CalledMethods == null)
                {
                    m_CalledMethods = new List<string>();
                }
                return m_CalledMethods;
            }
        }

        [TestFixtureSetUp]
        public void Setup1()
        {
            CalledMethods.Add("Setup1");
        }

        [TestFixtureSetUp]
        public void Setup2()
        {
            CalledMethods.Add("Setup2");
        }

        public bool SetupMethodsCalled()
        {
            if (CalledMethods.Count == 2 && 
                (CalledMethods.Contains("Setup1") && CalledMethods.Contains("Setup2")))
            {
                return true;
            }
            return false;
        }
    }
}
