﻿// Copyright 2017 Concordion.org
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
using System.Reflection;
using NUnit.Engine.Extensibility;

namespace Concordion.NUnit3
{
    [Extension]
    public class ConcordionFrameworkDriver : IDriverFactory
    {
        public bool IsSupportedTestFramework(AssemblyName reference)
        {
            throw new NotImplementedException();
        }

        public IFrameworkDriver GetDriver(AppDomain domain, AssemblyName reference)
        {
            throw new NotImplementedException();
        }
    }
}