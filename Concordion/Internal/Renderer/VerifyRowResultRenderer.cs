// Copyright 2009 Jeffrey Cameron
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Api.Listener;

namespace Concordion.Internal.Renderer
{
    public class VerifyRowResultRenderer : IVerifyRowsListener
    {
        #region IVerifyRowsListener Members

        public void ExpressionEvaluated(ExpressionEvaluatedEvent expressionEvaluatedEvent)
        {

        }

        public void MissingRow(MissingRowEvent missingRowEvent)
        {
            Element element = missingRowEvent.RowElement;
            element.AddStyleClass("missing");
        }

        public void SurplusRow(SurplusRowEvent surplusRowEvent)
        {
            Element element = surplusRowEvent.RowElement;
            element.AddStyleClass("surplus");
        }

        #endregion

    }
}
