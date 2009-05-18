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
    public class CommandCall
    {
        #region Properties
        
        public CommandCallList Children
        {
            get;
            private set;
        }

        public ICommand Command
        {
            get;
            private set;
        }

        public Resource Resource
        {
            get;
            private set;
        }

        public Element Element
        {
            get;
            set;
        }

        public string Expression
        {
            get;
            private set;
        }

        public bool HasChildCommands
        {
            get
            {
                return !Children.IsEmpty;
            }
        }

        #endregion

        #region Constructors
        
        public CommandCall(ICommand command, Element element, string expression, Resource resource)
        {
            Children = new CommandCallList();
            Command = command;
            Element = element;
            Expression = expression;
            Resource = resource;
        } 

        #endregion

        #region Methods

        public void SetUp(IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            Command.Setup(this, evaluator, resultRecorder);
        }

        public void Execute(IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            Command.Execute(this, evaluator, resultRecorder);
        }

        public void Verify(IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            Command.Verify(this, evaluator, resultRecorder);
        }

        public void AddChild(CommandCall child)
        {
            Children.Add(child);
        }

        #endregion
    }
}
