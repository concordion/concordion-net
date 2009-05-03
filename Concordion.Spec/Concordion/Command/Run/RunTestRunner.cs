using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Command.Run
{
    class RunTestRunner : IRunner
    {
        public static Result Result;

        public RunnerResult Execute(Resource resource, string href)
        {
		    return new RunnerResult(Result);
	    }
    }
}
