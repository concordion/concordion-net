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
//using System.Security.Policy;
using System.Text;
using Concordion.Api;
using System.IO;
using Concordion.Internal.Util;
using System.Drawing;
using System.Drawing.Imaging;

namespace Concordion.Internal
{
    public class FileTarget : ITarget
    {
        #region Fields

        private static readonly long FRESH_ENOUGH_MILLIS = 2000; // 2 secs

        #endregion

        #region Properties

        private string BaseDirectory
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public FileTarget(string baseDirectory)
        {
            if (baseDirectory.EndsWith("\\"))
            {
                BaseDirectory = baseDirectory;
            }
            else
            {
                BaseDirectory = baseDirectory + "\\";
            }
        }

        #endregion

        #region Methods

        private void MakeDirectories(Resource resource)
        {
            string path = Path.Combine(BaseDirectory, StripLeadingBackslash(resource.Parent.Path));
            Directory.CreateDirectory(path);
        }

        private static string StripLeadingBackslash(string path)
        {
            var strippedPath = path;
            if (strippedPath.StartsWith("\\"))
            {
                strippedPath = strippedPath.Remove(0, 1);
            }
            return strippedPath;
        }

        private StreamWriter CreateWriter(Resource resource)
        {

            string path = this.GetTargetPath(resource);
            return new StreamWriter(path, false, Encoding.UTF8);
        }

        private bool IsFreshEnough(string source)
        {
            TimeSpan ageInMillis = DateTime.Now.Subtract(File.GetLastWriteTime(source));
            return ageInMillis.TotalMilliseconds < FRESH_ENOUGH_MILLIS;
        }

        public string GetTargetPath(Resource resource)
        {
            return Path.Combine(this.BaseDirectory, resource.Path);
        }

        #endregion

        #region ITarget Members

        public void Write(Resource resource, string s)
        {
            Check.NotNull(resource, "resource is null");
            MakeDirectories(resource);
            using (StreamWriter writer = CreateWriter(resource))
            {
                try
                {
                    writer.Write(s);
                }
                finally { }
            }
        }

        public void Write(Resource resource, Bitmap image)
        {
            Check.NotNull(resource, "resource is null");
            MakeDirectories(resource);
            image.Save(Path.Combine(BaseDirectory, resource.Path), ImageFormat.Png);
        }

        public void CopyTo(Resource resource, string destination)
        {
            Check.NotNull(resource, "resource is null");
            MakeDirectories(resource);
            string source = BaseDirectory + resource.Path;

            if (File.Exists(source) && IsFreshEnough(source))
            {
                return;
            }
            File.Copy(source, destination);
        }

        public void CopyTo(Resource resource, TextReader inputReader)
        {
            Check.NotNull(resource, "resource is null");
            MakeDirectories(resource);
            var outputFile = GetTargetPath(resource);
            // Do not overwrite if a recent copy already exists
            if (File.Exists(outputFile) && IsFreshEnough(outputFile))
            {
                return;
            }
            IOUtil.Copy(inputReader, new StreamWriter(outputFile));
        }

        public void Delete(Resource resource)
        {
            Check.NotNull(resource, "resource is null");
            File.Delete(BaseDirectory + resource.Path);
        }

        public string ResolvedPathFor(Resource resource)
        {
            return GetTargetPath(resource);
        }

        #endregion
    }
}
