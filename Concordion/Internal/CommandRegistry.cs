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
    public class CommandRegistry : ICommandFactory
    {
        #region Fields
        
        private IDictionary<string, ICommand> m_commandMap; 

        #endregion

        #region Constructors
        
        public CommandRegistry()
        {
            m_commandMap = new Dictionary<string, ICommand>();
        } 

        #endregion

        #region Methods
        
        public CommandRegistry Register(string namespaceURI, string commandName, ICommand command)
        {
            m_commandMap.Add(MakeKey(namespaceURI, commandName), command);
            return this;
        }

        private string MakeKey(string namespaceURI, string commandName)
        {
            return namespaceURI + " " + commandName.ToLower();
        } 

        #endregion

        #region ICommandFactory Members

        public ICommand CreateCommand(string namespaceUri, string commandName)
        {
            return m_commandMap[MakeKey(namespaceUri, commandName)];
        }

        #endregion
    }
}
