using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Api.Extension
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExtensionsAttribute : Attribute
    {
        public Type[] ExtensionTypes { get; set; }

        public ExtensionsAttribute(params Type[] extensionTypes)
        {
            this.ExtensionTypes = extensionTypes;
        }
    }
}
