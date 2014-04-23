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
using System.Text.RegularExpressions;
using Concordion.Api;

namespace Concordion.Internal
{
    public class ClassNameBasedSpecificationLocator : ISpecificationLocator
    {
        #region ISpecificationLocator Members

        private string m_SpecificationSuffix;

        public ClassNameBasedSpecificationLocator() : this("html") { }

        public ClassNameBasedSpecificationLocator(string mSpecificationSuffix)
        {
            this.m_SpecificationSuffix = mSpecificationSuffix;
        }

        public Resource LocateSpecification(object fixture)
        {
            var fixtureName = fixture.GetType().ToString();
            fixtureName = fixtureName.Replace(".", "\\");

            //Todo:in Config File aufnehemn
            //Add Test und Fixture -> Case Sensitive 
            fixtureName = Regex.Replace(fixtureName, "(Fixture|Test)$", "");
            //Suffix from Concordion.Specification.config
            var path = fixtureName + "." + m_SpecificationSuffix;
            return new Resource(path);
        }

        #endregion
    }
}
