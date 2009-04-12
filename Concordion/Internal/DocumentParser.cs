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
using System.Xml.Linq;
using Concordion.Internal.Util;

namespace Concordion.Internal
{
    public class DocumentParser
    {
        #region Properties

        private ICommandFactory CommandFactory
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public DocumentParser(ICommandFactory commandFactory)
        {
            CommandFactory = commandFactory;
        }

        #endregion

        #region Methods

        public ISpecification Parse(XDocument document, Resource resource)
        {
            OnDocumentParsing(document);
            XElement rootElement = document.Root;
            CommandCall rootCommandCall = new CommandCall(CreateSpecificationCommand(), new Element(rootElement), "", resource);
            GenerateCommandCallTree(rootElement, rootCommandCall, resource);
            return new XmlSpecification(rootCommandCall);
        }

        private ICommand CreateSpecificationCommand()
        {
            ICommand specCmd = CreateCommand("", "specification");
            return specCmd;
        }

        private ICommand CreateCommand(string namespaceURI, string commandName)
        {
            return CommandFactory.CreateCommand(namespaceURI, commandName);
        }

        private void GenerateCommandCallTree(XElement element, CommandCall parentCommandCall, Resource resource)
        {
            bool isCommandAssigned = false;

            foreach (XAttribute attribute in element.Attributes())
            {
                string namespaceURI = attribute.Name.Namespace.NamespaceName;

                if (!attribute.IsNamespaceDeclaration && !String.IsNullOrEmpty(namespaceURI))
                {
                    string commandName = attribute.Name.LocalName;
                    ICommand command = CreateCommand(namespaceURI, commandName);
                    if (command != null)
                    {
                        Check.IsFalse(isCommandAssigned, "Multiple commands per element is currently not supported.");
                        isCommandAssigned = true;
                        String expression = attribute.Value;
                        CommandCall commandCall = new CommandCall(command, new Element(element), expression, resource);
                        parentCommandCall.AddChild(commandCall);
                        parentCommandCall = commandCall;
                    }
                }
            }

            foreach (XElement child in element.Elements())
            {
                GenerateCommandCallTree(child, parentCommandCall, resource);
            }
        }

        private void OnDocumentParsing(XDocument document)
        {
            if (DocumentParsing != null)
            {
                DocumentParsing(this, new DocumentParsingEventArgs { Document = document });
            }
        }

        #endregion

        #region Events

        public event EventHandler<DocumentParsingEventArgs> DocumentParsing;

        #endregion
    }
}
