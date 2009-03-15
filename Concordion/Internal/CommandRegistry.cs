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
            return namespaceURI + " " + commandName;
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
