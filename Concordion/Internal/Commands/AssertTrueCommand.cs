using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal.Commands
{
    public class AssertTrueCommand : BooleanCommand
    {
        protected override void ProcessTrueResult(CommandCall commandCall, global::Concordion.Api.IResultRecorder resultRecorder)
        {
            resultRecorder.Record(Result.Success);
            OnSuccessReported(commandCall.Element);
        }

        protected override void ProcessFalseResult(CommandCall commandCall, global::Concordion.Api.IResultRecorder resultRecorder)
        {
            resultRecorder.Record(Result.Failure);
            OnFailureReported(commandCall.Element, commandCall.Expression, "== false");
        }
    }
}
