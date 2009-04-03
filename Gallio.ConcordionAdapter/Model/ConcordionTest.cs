using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Model;
using Gallio.Reflection;
using Gallio.Model.Execution;
using Concordion.Api;

namespace Gallio.ConcordionAdapter.Model
{
    public class ConcordionTest : BaseTest
    {
        public ConcordionTypeInfoAdapter TypeInfo
        {
            get;
            private set;
        }

        public Resource Resource
        {
            get;
            private set;
        }

        public object Fixture
        {
            get;
            private set;
        }

        public ITarget Target
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new instance of a ConcordionTesr
        /// </summary>
        /// <param name="name"></param>
        /// <param name="codeElement"></param>
        /// 
        /// <param name="methodInfo"></param>
        public ConcordionTest(string name, ICodeElementInfo codeElement, ConcordionTypeInfoAdapter typeInfo, Resource resource, object fixture)
            : base(name, codeElement)
        {
            if (typeInfo == null)
                throw new ArgumentNullException(@"typeInfo");

            this.TypeInfo = typeInfo;
            this.Resource = resource;
            this.Fixture = fixture;
        }

        public override Func<ITestController> TestControllerFactory
        {
            get
            {
                return CreateTestController;
            }
        }

        private static ITestController CreateTestController()
        {
            return new ConcordionTestController();
        }
    }
}
