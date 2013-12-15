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
using System.Text.RegularExpressions;

namespace Concordion.Internal
{
    public class SimpleEvaluator : OgnlEvaluator
    {
        #region Fields

        private static readonly string METHOD_NAME_PATTERN = "[a-z][a-zA-Z0-9_]*";
        private static readonly string PROPERTY_NAME_PATTERN = "[a-z][a-zA-Z0-9_]*";
        private static readonly string STRING_PATTERN = "'[^']+'";
        private static readonly string LHS_VARIABLE_PATTERN = "#" + METHOD_NAME_PATTERN;
        private static readonly string RHS_VARIABLE_PATTERN = "(" + LHS_VARIABLE_PATTERN + "|#TEXT|#HREF|#LEVEL)";

        #endregion

        #region Properties



        #endregion

        #region Constructor
        
        public SimpleEvaluator(object fixture)
            : base(fixture)
        {
        } 

        #endregion

        #region Methods

        private void ValidateEvaluationExpression(string expression)
        {
            string METHOD_CALL_PARAMS = METHOD_NAME_PATTERN + " *\\( *" + RHS_VARIABLE_PATTERN + "(, *" + RHS_VARIABLE_PATTERN + " *)*\\)";
            string METHOD_CALL_NO_PARAMS = METHOD_NAME_PATTERN + " *\\( *\\)";
            string TERNARY_STRING_RESULT = " \\? " + STRING_PATTERN + " : " + STRING_PATTERN;
            
            List<string> regexPatterns = new List<string>();
            regexPatterns.Add(PROPERTY_NAME_PATTERN);
            regexPatterns.Add(METHOD_CALL_NO_PARAMS);
            regexPatterns.Add(METHOD_CALL_PARAMS);
            regexPatterns.Add(RHS_VARIABLE_PATTERN);
            regexPatterns.Add(LHS_VARIABLE_PATTERN + "(\\." + PROPERTY_NAME_PATTERN +  ")+");
            regexPatterns.Add(LHS_VARIABLE_PATTERN + " *= *" + PROPERTY_NAME_PATTERN);
            regexPatterns.Add(LHS_VARIABLE_PATTERN + " *= *" + METHOD_CALL_NO_PARAMS);
            regexPatterns.Add(LHS_VARIABLE_PATTERN + " *= *" + METHOD_CALL_PARAMS);
            regexPatterns.Add(LHS_VARIABLE_PATTERN + TERNARY_STRING_RESULT);
            regexPatterns.Add(PROPERTY_NAME_PATTERN + TERNARY_STRING_RESULT);
            regexPatterns.Add(METHOD_CALL_NO_PARAMS + TERNARY_STRING_RESULT);
            regexPatterns.Add(METHOD_CALL_PARAMS + TERNARY_STRING_RESULT);
            regexPatterns.Add(LHS_VARIABLE_PATTERN + "\\." + METHOD_CALL_NO_PARAMS);
            regexPatterns.Add(LHS_VARIABLE_PATTERN + "\\." + METHOD_CALL_PARAMS);
            
            expression = expression.Trim();

            foreach (string regexPattern in regexPatterns) 
            {
                if (Regex.IsMatch(expression, regexPattern)) 
                {
                    return;
                }
            }

            throw new InvalidOperationException("Invalid expression [" + expression + "]");
        }

        private void ValidateSetVariableExpression(string expression)
        {
            List<string> regexPatterns = new List<string>();
            regexPatterns.Add(RHS_VARIABLE_PATTERN);
            regexPatterns.Add(LHS_VARIABLE_PATTERN + "\\." + PROPERTY_NAME_PATTERN);
            regexPatterns.Add(LHS_VARIABLE_PATTERN + " *= *" + PROPERTY_NAME_PATTERN);
            regexPatterns.Add(LHS_VARIABLE_PATTERN + " *= *" + METHOD_NAME_PATTERN + " *\\( *\\)");
            regexPatterns.Add(LHS_VARIABLE_PATTERN + " *= *" + METHOD_NAME_PATTERN + " *\\( *" + RHS_VARIABLE_PATTERN + "(, *" + RHS_VARIABLE_PATTERN + " *)*\\)");

            expression = expression.Trim();

            foreach (string regexPattern in regexPatterns)
            {
                if (Regex.IsMatch(expression, regexPattern))
                {
                    return;
                }
            }

            throw new InvalidOperationException("Invalid expression [" + expression + "]");
        }

        #endregion

        #region Override Methods

        public override object  Evaluate(string expression)
        {
            ValidateEvaluationExpression(expression);
            return base.Evaluate(expression);
        }

        public override void SetVariable(string expression, object value)
        {
            ValidateSetVariableExpression(expression);
            base.SetVariable(expression, value);
        }


        #endregion
    }
}
