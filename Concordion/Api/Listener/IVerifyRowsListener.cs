using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api.Listener
{
    public interface IVerifyRowsListener
    {
        void ExpressionEvaluated(ExpressionEvaluatedEvent expressionEvaluatedEvent);
    
        void MissingRow(MissingRowEvent missingRowEvent);
    
        void SurplusRow(SurplusRowEvent surplusRowEvent);
    }
}
