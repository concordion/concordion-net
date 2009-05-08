using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Spec
{
    public class StubEvaluator : IEvaluator, IEvaluatorFactory
    {
        private object evaluationResult = null;

        public StubEvaluator(object fixture)
        {
            this.Fixture = fixture;
        }

        public IEvaluator CreateEvaluator(object fixture)
        {
            return this;
        }

        public object Evaluate(string expression) 
        {
            if (evaluationResult is Exception) 
            {
                throw (Exception) evaluationResult;
            }
            return evaluationResult;
        }

        public object GetVariable(string variableName)
        {
            return null;
        }

        public void SetVariable(string variableName, object value)
        {
        }

        public object Fixture
        {
            get;
            private set;
        }

        public IEvaluatorFactory withStubbedResult(object evaluationResult)
        {
            this.evaluationResult = evaluationResult;
            return this;
        }
    }
}