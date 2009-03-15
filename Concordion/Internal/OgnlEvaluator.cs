using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using ognl;
using Concordion.Internal.Util;
using System.Data;

namespace Concordion.Internal
{
    public class OgnlEvaluator : IEvaluator
    {
        #region Properties

        private object RootObject
        {
            get;
            set;
        }

        private OgnlContext OgnlContext
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public OgnlEvaluator(object rootObject)
        {
            RootObject = rootObject;
            OgnlContext = new OgnlContext();
        }

        #endregion

        #region Methods

        private void AssertStartsWithHash(string expression)
        {
            if (!expression.StartsWith("#"))
            {
                throw new InvalidExpressionException("Variable for concordion:set must start"
                        + " with '#'\n (i.e. change concordion:set=\"" + expression + "\" to concordion:set=\"#" + expression + "\".");
            }
        }

        private void PutVariable(string rawVariableName, object value)
        {
            Check.IsFalse(rawVariableName.StartsWith("#"), "Variable name passed to evaluator should not start with #");
            Check.IsTrue(!rawVariableName.Equals("in"), "'%s' is a reserved word and cannot be used for variables names", rawVariableName);
            OgnlContext[rawVariableName] = value;
        }

        #endregion

        #region IEvaluator Members

        public virtual object GetVariable(string expression)
        {
            AssertStartsWithHash(expression);
            string rawVariableName = expression.Substring(1);
            return OgnlContext[rawVariableName];
        }

        public virtual void SetVariable(string expression, object value)
        {
            AssertStartsWithHash(expression);
            if (expression.Contains("="))
            {
                Evaluate(expression);
            }
            else
            {
                String rawVariable = expression.Substring(1);
                PutVariable(rawVariable, value);
            }
        }

        public virtual object Evaluate(string expression)
        {
            Check.NotNull(RootObject, "Root object is null");
            Check.NotNull(expression, "Expression to evaluate cannot be null");
            return Ognl.getValue(expression, OgnlContext, RootObject);
        }

        #endregion
    }
}
