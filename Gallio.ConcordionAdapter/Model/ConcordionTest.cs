using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Model;
using Concordion.Api;
using Gallio.Common.Reflection;
using Gallio.Common;
using Gallio.Model.Commands;
using Gallio.Model.Tree;

namespace Gallio.ConcordionAdapter.Model
{
    /// <summary>
    /// 
    /// </summary>
    internal class ConcordionTest : Test
    {
        #region Properties

        /// <summary>
        /// Gets or sets the type info.
        /// </summary>
        /// <value>The type info.</value>
        public ConcordionTypeInfoAdapter TypeInfo
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        /// <value>The resource.</value>
        public Resource Resource
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the fixture.
        /// </summary>
        /// <value>The fixture.</value>
        public Type FixtureType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public ISource Source
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        public ITarget Target
        {
            get;
            set;
        } 

        #endregion

        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ConcordionTest"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="codeElement">The code element.</param>
        /// <param name="typeInfo">The type info.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="fixtureType">The fixture type.</param>
     
        public ConcordionTest(string name, ICodeElementInfo codeElement, ConcordionTypeInfoAdapter typeInfo, Resource resource, Type fixtureType)
            : base(name, codeElement)
        {
            if (typeInfo == null)
                throw new ArgumentNullException(@"typeInfo");

            this.TypeInfo = typeInfo;
            this.Resource = resource;
            this.FixtureType = fixtureType;
        } 

        #endregion

        #region Methods
        
        /*
        private static ITestController CreateTestController()
        {
            return new ConcordionTestController();
        } 
         */

        #endregion

        //#region Override Methods

        /// <summary>
        /// Gets the test controller factory.
        /// </summary>
        /// <value>The test controller factory.</value>
        /*
        public override Gallio.Common.Func<ITestController> TestControllerFactory
        {
            get
            {
                return CreateTestController;
            }
        } 
         */

       // #endregion
    }
}
