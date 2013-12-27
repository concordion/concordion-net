using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api.Extension
{
    public interface IConcordionExtension
    {
        void AddTo(IConcordionExtender concordionExtender);
    }
}
