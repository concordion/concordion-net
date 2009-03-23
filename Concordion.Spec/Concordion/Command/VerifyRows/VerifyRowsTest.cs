using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Internal.Util;
using Concordion.Api;
using System.Xml.Linq;

namespace Concordion.Spec.Concordion.Command.VerifyRows
{
    class VerifyRowsTest
    {
        public ICollection<string> usernames;

        // TODO - repair this
        //public string processFragment(string fragment, string csv)
        //{
        //    usernames = csvToCollection(csv);
        //    XDocument document = new TestRig()
        //        .WithFixture(this)
        //        .ProcessFragment(fragment)
        //        .GetXOMDocument();

        //    String result = "";
        //    XElement table = document.query("//table").get(0);
        //    XNode rows = table.Query(".//tr");
        //    for (int i = 1; i < rows.size(); i++) {
        //        if (!result.Equals("")) {
        //            result += ", ";
        //        }
        //        result += categorize((Element)rows.get(i));
        //    }

        //    return result;
        //}

        //private string categorize(Element row) 
        //{
        //    string cssClass = row.getAttributeValue("class");
        //    if (cssClass == null) {
        //        Element cell = (Element) row.query("td").get(0);
        //        cssClass = cell.getAttributeValue("class");
        //    }
        //    Check.NotNull(cssClass, "cssClass is null");
        //    return cssClass.ToUpper();
        //}

        private static ICollection<string> csvToCollection(string csv) 
        {
            ICollection<string> c = new List<string>();
            foreach (string s in csv.Split(',', ' ')) {
                c.Add(s);
            }
            return c;
        }
    }
}
