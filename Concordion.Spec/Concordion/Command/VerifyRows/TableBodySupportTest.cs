using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Spec.Concordion.Command.VerifyRows
{
    class TableBodySupportTest
    {
        private List<string> names = new List<string>();

        public void setUpNames(string namesAsCSV) 
        {
            foreach (string name in namesAsCSV.Split(',', ' '))
            {
                names.Add(name);
            }
        }

        public List<string> getNames()
        {
            return names;
        }

        public string process(string inputFragment)
        {
            return new TestRig()
                .WithFixture(this)
                .ProcessFragment(inputFragment).SuccessOrFailureInWords();
                //TODO - repair this .GetXOMDocument()
                //.Query("//table").get(0)
                //.ToXML()
                //.Replace("\u00A0", "&#160;");
        }
    }
}
