using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Concordion.Integration;
using Concordion.Spec.Support;

namespace Concordion.Spec.Concordion.Command.AssertEquals.Whitespace
{
    [ConcordionTest]
    public class LineContinuationsTest
    {
        private List<string> snippets = new List<string>();

        public void addSnippet(string snippet)
        {
            snippets.Add(snippet);
        }

        public Result processSnippets(string evaluationResult)
        {
            Result result = new Result();

            int i = 1;
            foreach (string snippet in snippets) 
            {
                if (new TestRig()
                        .WithStubbedEvaluationResult(evaluationResult)
                        .ProcessFragment(snippet)
                        .HasFailures) 
                {
                    result.failures += "(" + i + "), ";
                } 
                else 
                {
                    result.successes += "(" + i + "), ";
                }
                i++;
            }
            result.failures = Regex.Replace(result.failures, ", $", "");
            result.successes = Regex.Replace(result.successes, ", $", "");
            
            return result;
        }

        public class Result
        {
            public string successes = "";
            public string failures = "";
        }
    }
}
