using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Integration;
using System.Xml.Linq;

namespace Concordion.Spec.Concordion.Command.VerifyRows
{
    [ConcordionTest]
    public class TableBodySupportTest
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
            var document = new TestRig()
                                .WithFixture(this)
                                .ProcessFragment(inputFragment)
                                .GetXDocument();

            var table = document.Element("html").Element("body").Element("fragment").Element("table");

            return table.ToString(SaveOptions.DisableFormatting).Replace("\u00A0", "&#160;");
        }
    }
}
