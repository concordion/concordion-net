using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api
{
    public class RunnerResult
    {
        public Result Result
        {
            get;
            private set;
        }

        public RunnerResult(Result result) 
        {
            this.Result = result;
        }
        
        public Result getResult() 
        {
            return Result;
        }
    }
}
