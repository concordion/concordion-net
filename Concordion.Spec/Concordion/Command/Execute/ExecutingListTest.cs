using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Command.Execute
{
    [ConcordionTest]
    public class ExecutingListTest
    {
        private Queue<ParseParameters> nodes = new Queue<ParseParameters>();

        public void ParseNode(string text, int level)
        {
            ParseParameters parseParameters = new ParseParameters();
            parseParameters.Name = text;
            parseParameters.Level = level;
            nodes.Enqueue(parseParameters);
        }

        public struct ParseParameters
        {
            public string Name;
            public int Level;
        }

        public Queue<ParseParameters> GetNodes()
        {
            return nodes;
        }

        public void process(string fragment)
        {
            ProcessingResult r = new TestRig()
                .WithFixture(this)
                .ProcessFragment(fragment);
        }
    }
}
