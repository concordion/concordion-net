using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class ResultDetails
    {
        public Result Result { get; private set; }

        public string Message { get; private set; }

        public string StackTrace { get; private set; }

        public Exception Exception { get; private set; }

        public ResultDetails(Result result, string message, string stackTrace)
        {
            this.Result = result;
            this.Message = message;
            this.StackTrace = stackTrace;
        }

        public ResultDetails(Result result, Exception exception)
        {
            this.Result = result;
            this.Exception = exception;
        }

        public ResultDetails(Result result)
        {
            this.Result = result;
        }

        public bool IsSuccess
        {
            get { return this.Result == Result.Success; }
        }
        
        public bool IsFailure
        {
            get { return this.Result == Result.Failure; }
        }

        public bool IsError
        {
            get { return this.Result == Result.Exception; }
        }
    }
}
