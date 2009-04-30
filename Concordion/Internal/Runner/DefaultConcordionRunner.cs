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
using Gallio.Runner;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Concordion.Internal.Runner
{
    public class DefaultConcordionRunner : IRunner
    {
        #region Properties

        public ISource Source
        {
            get;
            private set;
        }

        public ITarget Target
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public DefaultConcordionRunner(ISource source, ITarget target)
        {
            this.Source = source;
            this.Target = target;
        }

        #endregion

        #region Methods

        private bool IsOnlySuccessfulBecauseItWasExpectedToFail(Type concordionClass) 
        {
            //return concordionClass.getAnnotation(ExpectedToFail.class) != null;
            return true;
        }

        #endregion

        #region IRunner Members

        public RunnerResult Execute(Resource resource, string href)
        {
            var config = new ConcordionConfig();
            config.Load();

            var fixture = GetFixture(resource, href, config);

            var concordion = new ConcordionBuilder()
                                        .WithSource(Source)
                                        .WithTarget(Target)
                                        .Build();

            var results = concordion.Process(fixture);

            Result result;
            if (results.HasFailures)
            {
                result = Result.Failure;
            }
            else if (results.HasExceptions)
            {
                result = Result.Exception;
            }
            else
            {
                result = Result.Success;
            }

		    return new RunnerResult(result);
        }

        private static object GetFixture(Resource resource, string href, ConcordionConfig config)
        {
            Resource hrefResource = resource.GetRelativeResource(href);
            var fixturePath = hrefResource.Path;
            var fixtureFullyQualifiedPath = fixturePath.Replace("\\", ".");
            var fixtureName = fixtureFullyQualifiedPath.Replace(".html", "Test");

            if (fixtureName.StartsWith("."))
            {
                fixtureName = fixtureName.Remove(0, 1);
            }

            foreach (var fullyQualifiedAssemblyName in config.SpecificationAssemblies)
            {
                var specificationAssembly = Assembly.Load(fullyQualifiedAssemblyName);
                var fixtureType = specificationAssembly.GetType(fixtureName, false, true);

                if (fixtureType != null)
                {
                    if (fixtureType.IsClass)
                    {
                        var constructor = fixtureType.GetConstructor(Type.EmptyTypes);

                        if (constructor != null)
                        {
                            return constructor.Invoke(new Object[] { });
                        }
                    }
                }
                else
                {
                    fixtureName = "Concordion." + fixtureName;

                    fixtureType = specificationAssembly.GetType(fixtureName, false, true);

                    if (fixtureType != null)
                    {
                        if (fixtureType.IsClass)
                        {
                            var constructor = fixtureType.GetConstructor(Type.EmptyTypes);

                            if (constructor != null)
                            {
                                return constructor.Invoke(new Object[] { });
                            }
                        }
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
