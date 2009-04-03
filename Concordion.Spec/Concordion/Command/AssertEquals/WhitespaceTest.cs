using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Internal;
using System.Text.RegularExpressions;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Command.AssertEquals.Whitespace
{
    [ConcordionTest]
    public class WhitespaceTest
    {
        public string whichSnippetsSucceed(string snippet1, string snippet2, string evaluationResult) 
        {
            return which(succeeds(snippet1, evaluationResult), succeeds(snippet2, evaluationResult));
        }

        public string whichSnippetsFail(string snippet1, string snippet2, string evaluationResult) 
        {
            return which(fails(snippet1, evaluationResult), fails(snippet2, evaluationResult));
        }

        private static string which(bool b1, bool b2) 
        {
            if (b1 && b2) {
                return "both";
            } else if (b1) {
                return "the first of";
            } else if (b2) {
                return "the second of";
            }
            return "neither";
        }

        private bool fails(string snippet, string evaluationResult)
        { 
            return !succeeds(snippet, evaluationResult);
        }

        private bool succeeds(string snippet, string evaluationResult)
        {
            return new TestRig()
                .WithStubbedEvaluationResult(evaluationResult)
                .ProcessFragment(snippet)
                .IsSuccess;
        }

        public string normalize(string s) 
        {
            // Bit naughty calling internal method normalize() directly 
            return replaceRealWhitespaceCharactersWithNames(
                    BrowserStyleWhitespaceComparer.Normalize(replaceNamedWhitespaceWithRealWhitespaceCharacters(s)));
        }

        private static string replaceNamedWhitespaceWithRealWhitespaceCharacters(string s) 
        {
            var spacesRemoved = Regex.Replace(s, "\\[SPACE\\]", " ");
            var tabsRemoved = Regex.Replace(spacesRemoved, "\\[TAB\\]", "\t");
            var lfRemoved = Regex.Replace(tabsRemoved, "\\[LF\\]", "\n");
            return Regex.Replace(lfRemoved, "\\[CR\\]", "\r");
        }

        private static string replaceRealWhitespaceCharactersWithNames(string s) 
        {
            return s.Replace(" ", "[SPACE]");
        }
    }
}
