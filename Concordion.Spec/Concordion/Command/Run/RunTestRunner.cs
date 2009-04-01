using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Command.Run
{
    class RunTestRunner
    {
        public static Result result;
	    private string param = String.Empty;
    	
	    public void setTestParam(string param){
		    this.param  = param;
	    }

        public RunnerResult execute(Resource resource, string href)
        {
		    if (param != href)
            {
			    throw new Exception("testParam not set");
		    }
		    return new RunnerResult(result);
	    }
    }
}
