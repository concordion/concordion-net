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
using Concordion.Api.Listener;
using Concordion.Internal.Commands;
using Concordion.Api;
using Concordion.Internal.Util;
using Concordion.Internal.Renderer;
using System.IO;
using Concordion.Internal.Runner;

namespace Concordion.Internal
{
    public class ConcordionBuilder
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

        #endregion
    
        public ConcordionBuilder()
        {
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

            var documentStructureImprovementRenderer = new DocumentStructureImprovementRenderer();
            DocumentParser.DocumentParsing += documentStructureImprovementRenderer.DocumentParsingEventHandler;

            var stylesheetEmbeddingRenderer = new StylesheetEmbeddingRenderer(HtmlFramework.EMBEDDED_STYLESHEET_RESOURCE);
            DocumentParser.DocumentParsing += stylesheetEmbeddingRenderer.DocumentParsingEventHandler;
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

            var breadCrumbRenderer = new BreadCrumbRenderer(Source);
            SpecificationCommand.SpecificationCommandProcessing += breadCrumbRenderer.SpecificationProcessingEventHandler;
            SpecificationCommand.SpecificationCommandProcessed += breadCrumbRenderer.SpecificationProcessedEventHandler;

            var pageFooterRenderer = new PageFooterRenderer(Target);
            SpecificationCommand.SpecificationCommandProcessing += pageFooterRenderer.SpecificationProcessingEventHandler;
            SpecificationCommand.SpecificationCommandProcessed += pageFooterRenderer.SpecificationProcessedEventHandler;

            var specificationRenderer = new SpecificationRenderer(Target);
            SpecificationCommand.SpecificationCommandProcessing += specificationRenderer.SpecificationProcessingEventHandler;
            SpecificationCommand.SpecificationCommandProcessed += specificationRenderer.SpecificationProcessedEventHandler;

            SpecificationReader = new XmlSpecificationReader(Source, DocumentParser);        

            return new Concordion(SpecificationLocator, SpecificationReader, EvaluatorFactory);
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

        //public ConcordionBuilder WithAssertEqualsListener(IAssertEqualsListener eventRecorder)
        //{
        //    AssertEqualsCommand.SuccessReported += eventRecorder.SuccessReportedEventHandler;
        //    AssertEqualsCommand.FailureReported += eventRecorder.FailureReportedEventHandler;
        //    AssertTrueCommand.SuccessReported += eventRecorder.SuccessReportedEventHandler;
        //    AssertTrueCommand.FailureReported += eventRecorder.FailureReportedEventHandler;
        //    AssertFalseCommand.SuccessReported += eventRecorder.SuccessReportedEventHandler;
        //    AssertFalseCommand.FailureReported += eventRecorder.FailureReportedEventHandler;
        //    return this;
        //}

        public ConcordionBuilder WithExceptionListener(IExceptionCaughtListener listener)
        {
            // TODO - add code here for processing
            return this;
        }

        public ConcordionBuilder WithSpecificationListener(ISpecificationListener listener)
        {
            SpecificationCommand.SpecificationCommandProcessing += listener.SpecificationProcessingEventHandler;
            SpecificationCommand.SpecificationCommandProcessed += listener.SpecificationProcessedEventHandler;
            return this;
        }
    }
}
