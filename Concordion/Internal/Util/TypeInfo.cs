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

namespace Concordion.Internal.Util
{
    public static class TypeInfo
    {
        public static T CreateInstance<T>(string assemblyQualifiedName)
        {
            var concordionRunnerType = Type.GetType(assemblyQualifiedName);
            var concordionRunnerTypeConstructor = concordionRunnerType.GetConstructor(System.Type.EmptyTypes);
            var concordionRunnerImpl = (T)concordionRunnerTypeConstructor.Invoke(null);

            return concordionRunnerImpl;
        }
    }
}
