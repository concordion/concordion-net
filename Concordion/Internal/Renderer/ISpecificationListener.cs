using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Internal.Commands;

namespace Concordion.Internal.Renderer
{
    public interface ISpecificationListener
    {
        void SpecificationProcessingEventHandler(object sender, SpecificationEventArgs eventArgs);
        void SpecificationProcessedEventHandler(object sender, SpecificationEventArgs eventArgs);
    }
}
