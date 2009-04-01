using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Internal.Util;
using Concordion.Api;
using System.Xml.Linq;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Command.VerifyRows
{
    [ConcordionTest]
    public class VerifyRowsTest
    {
        public ICollection<string> usernames = new List<string>();

        public string processFragment(string fragment, string csv)
        {
            usernames = csvToCollection(csv);
            XDocument document = new TestRig()
                .WithFixture(this)
                .ProcessFragment(fragment)
                .GetXDocument();

            var result = String.Empty;

            var table = document.Descendants("table").ToList()[0];
            var rows = table.Elements("tr");

            for (int index = 1; index < rows.Count(); index++)
            {
                var row = rows.ToArray()[index];
                if (!String.IsNullOrEmpty(result))
                {
                    result += ", ";
                }
                result += categorize(row);
            }

            return result;
        }

        private string categorize(XElement row)
        {
            var cssClass = row.Attribute("class");
            if (cssClass == null)
            {
                var cell = row.Element("td");
                cssClass = cell.Attribute("class");
            }
            Check.NotNull(cssClass, "cssClass is null");
            return cssClass.Value.ToUpper();
        }

        private static ICollection<string> csvToCollection(string csv) 
        {
            ICollection<string> c = new List<string>();
            foreach (string s in csv.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)) 
            {
                c.Add(s);
            }
            return c;
        }
    }
}
