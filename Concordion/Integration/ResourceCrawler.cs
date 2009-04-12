// Copyright 2009 Jeffrey Cameron
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
            var resources = new List<string>();
            var stack = new Stack<string>();
            stack.Push(BaseDirectory);

            while (stack.Count > 0)
            {
                string dir = stack.Pop();

                resources.AddRange(Directory.GetFiles(dir, "*.html"));

                foreach (string dn in Directory.GetDirectories(dir))
                {
                    stack.Push(dn);
                }
            }

            //return resources;

            var relativeResources = new List<string>();
            foreach (string resource in resources)
            {
                relativeResources.Add(resource.Replace(Path.GetPathRoot(resource), "\\"));
            }

            return relativeResources;
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
