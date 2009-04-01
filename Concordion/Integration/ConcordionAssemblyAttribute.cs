using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Concordion.Integration
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ConcordionAssemblyAttribute : Attribute
    {
        public DirectoryInfo BaseInputDirectory
        {
            get;
            set;
        }

        public DirectoryInfo BaseOutputDirectory
        {
            get;
            set;
        }

        public ConcordionAssemblyAttribute(string inputPath, string outputPath)
        {
            BaseInputDirectory = new DirectoryInfo(inputPath);
            BaseOutputDirectory = new DirectoryInfo(outputPath);
        }
    }
}
