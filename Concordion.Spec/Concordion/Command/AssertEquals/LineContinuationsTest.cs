using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Spec.Concordion.Command.AssertEquals
{
    class LineContinuationsTest
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
            result.failures = result.failures.Replace(", $", "");
            result.successes = result.successes.Replace(", $", "");
            
            return result;
        }

        public class Result
        {
            public string successes = "";
            public string failures = "";
        }
    }
}
