using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Integration
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HtmlResourceAttribute : Attribute
    {
        public string Name
        {
            get;
            private set;
        }

        public string Path
        {
            get;
            private set;
        }

        public HtmlResourceAttribute(string name)
        {
            this.Name = name;
        }

        public HtmlResourceAttribute(string name, string path)
            : this(name)
        {
            this.Path = path;
        }
    }
}
