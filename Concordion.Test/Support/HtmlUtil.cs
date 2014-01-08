using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Concordion.Test.Support
{
    public class HtmlUtil
    {
        public string RemoveWhitespaceBetweenTags(string inputString)
        {
            return Regex.Replace(inputString, ">[\r\n ]*<", "><");
        }
    }
}
