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
using Concordion.Api.Extension;
using Concordion.Api.Listener;
using Concordion.Internal.Commands;
using Concordion.Api;
using Concordion.Internal.Listener;
using Concordion.Internal.Util;
using Concordion.Internal.Renderer;
using System.IO;
using Concordion.Internal.Runner;

namespace Concordion.Internal
{
    public class ConcordionBuilder : IConcordionExtender
    {
        #region Properties

        private ExceptionRenderer ExceptionRenderer
        {
            get;
            set;
        }

        private string BaseOutputDir
        {
            get;
            set;
        }

        private ISource Source
        {
            get;
            set;
        }

        private ITarget Target
        {
            get;
            set;
        }

        private ISpecificationLocator SpecificationLocator
        {
            get;
            set;
        }

        private CommandRegistry CommandRegistry
        {
            get;
            set;
        }

        private DocumentParser DocumentParser
        {
            get;
            set;
        }

        private ISpecificationReader SpecificationReader
        {
            get;
            set;
        }

        private IEvaluatorFactory EvaluatorFactory
        {
            get;
            set;
        }

        private SpecificationCommand SpecificationCommand
        {
            get;
            set;
        }

        private AssertEqualsCommand AssertEqualsCommand
        {
            get;
            set;
        }

        private AssertTrueCommand AssertTrueCommand
        {
            get;
            set;
        }

        private AssertFalseCommand AssertFalseCommand
        {
            get;
            set;
        }

        private ExecuteCommand ExecuteCommand
        {
            get;
            set;
        }

        private RunCommand RunCommand
        {
            get;
            set;
        }

        private VerifyRowsCommand VerifyRowsCommand
        {
            get;
            set;
        }

        private EchoCommand EchoCommand
        {
            get;
            set;
        }

        private List<IConcordionBuildListener> BuildListeners
        {
            get;
            set;
        }

        private List<ISpecificationProcessingListener> SpecificationProcessingListeners
        {
            get;
            set;
        }

        private Dictionary<string, Resource> ResourceToCopyMap
        {
            get;
            set;
        } 

        #endregion
    
        public ConcordionBuilder()
        {
            BuildListeners = new List<IConcordionBuildListener>();
            SpecificationProcessingListeners = new List<ISpecificationProcessingListener>();
            ResourceToCopyMap = new Dictionary<string, Resource>();

            SpecificationLocator = new ClassNameBasedSpecificationLocator();
            Source = null;
            Target = null;
            CommandRegistry = new CommandRegistry();
            DocumentParser = new DocumentParser(CommandRegistry);
            EvaluatorFactory = new SimpleEvaluatorFactory();
            SpecificationCommand = new SpecificationCommand();
            AssertEqualsCommand = new AssertEqualsCommand();
            AssertTrueCommand = new AssertTrueCommand();
            AssertFalseCommand = new AssertFalseCommand();
            ExecuteCommand = new ExecuteCommand();
            RunCommand = new RunCommand();
            VerifyRowsCommand = new VerifyRowsCommand();
            EchoCommand = new EchoCommand();
            ExceptionRenderer = new ExceptionRenderer();

            // Set up the commands
            
            CommandRegistry.Register("", "specification", SpecificationCommand);
            WithApprovedCommand(HtmlFramework.NAMESPACE_CONCORDION_2007, "run", RunCommand);
            WithApprovedCommand(HtmlFramework.NAMESPACE_CONCORDION_2007, "execute", ExecuteCommand);
            WithApprovedCommand(HtmlFramework.NAMESPACE_CONCORDION_2007, "set", new SetCommand());
            WithApprovedCommand(HtmlFramework.NAMESPACE_CONCORDION_2007, "assertEquals", AssertEqualsCommand);
            WithApprovedCommand(HtmlFramework.NAMESPACE_CONCORDION_2007, "assertTrue", AssertTrueCommand);
            WithApprovedCommand(HtmlFramework.NAMESPACE_CONCORDION_2007, "assertFalse", AssertFalseCommand);
            WithApprovedCommand(HtmlFramework.NAMESPACE_CONCORDION_2007, "verifyRows", VerifyRowsCommand);
            WithApprovedCommand(HtmlFramework.NAMESPACE_CONCORDION_2007, "echo", EchoCommand);

            // Wire up the command listeners

            var assertResultRenderer = new AssertResultRenderer();
            WithAssertEqualsListener(assertResultRenderer);
            WithAssertTrueListener(assertResultRenderer);
            WithAssertFalseListener(assertResultRenderer);

            var verifyRowsCommandRenderer = new VerifyRowResultRenderer();
            VerifyRowsCommand.MissingRowFound += verifyRowsCommandRenderer.MissingRowFoundEventHandler;
            VerifyRowsCommand.SurplusRowFound += verifyRowsCommandRenderer.SurplusRowFoundEventHandler;

            var runResultRenderer = new RunResultRenderer();
            RunCommand.SuccessfulRunReported += runResultRenderer.SuccessfulRunReportedEventHandler;
            RunCommand.FailedRunReported += runResultRenderer.FailedRunReportedEventHandler;
            RunCommand.IgnoredRunReported += runResultRenderer.IgnoredRunReportedEventHandler;

            WithDocumentParsingListener(new DocumentStructureImprover());
            WithEmbeddedCss(HtmlFramework.EMBEDDED_STYLESHEET_RESOURCE);
        }
    
        public ConcordionBuilder WithSource(ISource source) 
        {
            this.Source = source;
            return this;
        }

        public ConcordionBuilder WithTarget(ITarget target) 
        {
            this.Target = target;
            return this;
        }

        public ConcordionBuilder WithEvaluatorFactory(IEvaluatorFactory evaluatorFactory) 
        {
            this.EvaluatorFactory = evaluatorFactory;
            return this;
        }

        public ConcordionBuilder WithExceptionRenderer(ExceptionRenderer exceptionRendererToAttach)
        {
            ExceptionRenderer = exceptionRendererToAttach;
            return this;
        }

        public ConcordionBuilder WithAssertEqualsListener(IAssertEqualsListener listener)
        {
            AssertEqualsCommand.AddAssertEqualsListener(listener);
            return this;
        }

        public ConcordionBuilder WithAssertTrueListener(IAssertTrueListener listener)
        {
            AssertTrueCommand.AddAssertListener(listener);
            return this;
        }

        public ConcordionBuilder WithAssertFalseListener(IAssertFalseListener listener)
        {
            AssertFalseCommand.AddAssertListener(listener);
            return this;
        }

        private ConcordionBuilder WithApprovedCommand(string namespaceURI, string commandName, ICommand command) 
        {
            ExceptionCatchingDecorator ExceptionCatchingDecorator = new ExceptionCatchingDecorator(new LocalTextDecorator(command));
            ExceptionCatchingDecorator.ExceptionCaught += ExceptionRenderer.ExceptionCaughtEventHandler;
            ICommand decoratedCommand = ExceptionCatchingDecorator;
            CommandRegistry.Register(namespaceURI, commandName, decoratedCommand);
            return this;
        }

        public ConcordionBuilder WithDocumentParsingListener(IDocumentParsingListener listener)
        {
            DocumentParser.AddDocumentParsingListener(listener);
            return this;
        }

        public ConcordionBuilder WithSpecificationProcessingListener(ISpecificationProcessingListener listener)
        {
            SpecificationProcessingListeners.Add(listener);
            return this;
        }

        public ConcordionBuilder WithBuildListener(IConcordionBuildListener listener)
        {
            BuildListeners.Add(listener);
            return this;
        }

        public ConcordionBuilder WithCommand(string namespaceURI, string commandName, ICommand command) 
        {
            Check.NotEmpty(namespaceURI, "Namespace URI is mandatory");
            Check.NotEmpty(commandName, "Command name is mandatory");
            Check.NotNull(command, "Command is null");
            Check.IsFalse(namespaceURI.Contains("concordion.org"),
                    "The namespace URI for user-contributed command '" + commandName + "' "
                  + "must not contain 'concordion.org'. Use your own domain name instead.");
            return WithApprovedCommand(namespaceURI, commandName, command);
        }

        public ConcordionBuilder WithResource(String sourcePath, Resource targetResource)
        {
            ResourceToCopyMap.Add(sourcePath, targetResource);
            return this;
        }

        public ConcordionBuilder WithEmbeddedCss(string css)
        {
            var embedder = new StylesheetEmbedder(css);
            WithDocumentParsingListener(embedder);
            return this;
        }

        public IConcordionExtender WithEmbeddedJavaScript(string javaScript)
        {
            var embedder = new JavaScriptEmbedder(javaScript);
            WithDocumentParsingListener(embedder);
            return this;
        }

        public IConcordionExtender WithLinkedJavaScript(string jsPath, Resource targetResource)
        {
            WithResource(jsPath, targetResource);
            var javaScriptLinker = new JavaScriptLinker(targetResource);
            WithDocumentParsingListener(javaScriptLinker);
            WithSpecificationProcessingListener(javaScriptLinker);
            return this;
        }

        public ConcordionBuilder SendOutputTo(string directory)
        {
            Target = new FileTarget(directory);
            return this;
        }
        
        public Concordion Build() 
        {
            if (Target == null)
            {
                Target = new FileTarget(BaseOutputDir ?? Directory.GetCurrentDirectory());
            }

            SetAllRunners();

            SpecificationCommand.AddSpecificationListener(new BreadCrumbRenderer(this.Source));
            SpecificationCommand.AddSpecificationListener(new PageFooterRenderer(this.Target));
            SpecificationCommand.AddSpecificationListener(new SpecificationRenderer(this.Target));

            SpecificationReader = new XmlSpecificationReader(Source, DocumentParser);

            CopyResources();

            AddSpecificationListeners();

            foreach (var concordionBuildListener in BuildListeners)
            {
                concordionBuildListener.ConcordionBuilt(new ConcordionBuildEvent(Target));
            }

            return new Concordion(SpecificationLocator, SpecificationReader, EvaluatorFactory);
        }

        private void AddSpecificationListeners()
        {
            foreach (var listener in SpecificationProcessingListeners)
            {
                SpecificationCommand.AddSpecificationListener(listener);
            }
        }

        private void CopyResources()
        {
            foreach (var resource in ResourceToCopyMap)
            {
                var sourcePath = resource.Key;
                var targetResource = resource.Value;
                var inputReader = Source.CreateReader(new Resource(sourcePath));
                Target.CopyTo(targetResource, inputReader);
            }
        }

        private void SetAllRunners()
        {
            RunCommand.Runners.Add("concordion", new DefaultConcordionRunner(Source, Target));

            var config = new ConcordionConfig().Load();

            foreach (var runner in config.Runners)
            {
                RunCommand.Runners.Add(runner.Key, runner.Value);
            }
        }

        public ConcordionBuilder WithExceptionListener(IExceptionCaughtListener listener)
        {
            // TODO - add code here for processing
            return this;
        }
    }
}
