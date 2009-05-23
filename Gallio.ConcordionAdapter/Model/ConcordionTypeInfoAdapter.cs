using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Common.Reflection;

namespace Gallio.ConcordionAdapter.Model
{
    /// <summary>
    /// Adapts type information for Concordion
    /// </summary>
    public class ConcordionTypeInfoAdapter
    {
        #region Properties
        
        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        public ITypeInfo Target
        {
            get;
            private set;
        } 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcordionTypeInfoAdapter"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        public ConcordionTypeInfoAdapter(ITypeInfo target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            Target = target;
        } 

        #endregion
    }
}
