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
using Concordion.Api;

namespace Concordion.Internal
{
    public class ClassNameBasedSpecificationLocator : ISpecificationLocator
    {
        #region ISpecificationLocator Members

        public Resource LocateSpecification(object fixture)
        {
            var fixtureName = fixture.GetType().ToString();
            fixtureName = fixtureName.Replace(".", "\\");
            fixtureName = fixtureName.Remove(fixtureName.Length - 4);

            var path = fixtureName + ".html";

            return new Resource(path);
        }

        #endregion
    }
}
