using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Concordion.Integration
{
    public class ResourceCrawler : IEnumerable<string>
    {
        #region Properties

        private string BaseDirectory
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public ResourceCrawler(string baseDirectory)
        {
            if (Directory.Exists(baseDirectory))
            {
                BaseDirectory = baseDirectory;
            }
            else
            {
                throw new ArgumentException("The base directory must exist", "baseDirectory");
            }
        }

        #endregion

        #region Methods

        public List<string> GetFilesRecursive()
        {
            List<string> result = new List<string>();
            Stack<string> stack = new Stack<string>();
            stack.Push(BaseDirectory);

            while (stack.Count > 0)
            {
                string dir = stack.Pop();

                result.AddRange(Directory.GetFiles(dir, "*.html"));

                foreach (string dn in Directory.GetDirectories(dir))
                {
                    stack.Push(dn);
                }
            }
            return result;
        }

        #endregion

        #region IEnumerable<string> Members

        public IEnumerator<string> GetEnumerator()
        {
            return GetFilesRecursive().GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetFilesRecursive().GetEnumerator();
        }

        #endregion
    }
}
