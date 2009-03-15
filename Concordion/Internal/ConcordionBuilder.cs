using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Internal.Commands;
using Concordion.Api;
using Concordion.Internal.Util;
using System.IO;

namespace Concordion.Internal
{
    public class ConcordionBuilder
    {
        private static readonly string NAMESPACE_CONCORDION_2007 = "http://www.concordion.org/2007/concordion";
        private static readonly string PROPERTY_OUTPUT_DIR = "concordion.output.dir";
        private static readonly string EMBEDDED_STYLESHEET_RESOURCE = "/org/concordion/internal/resource/embedded.css";

        private ISpecificationLocator specificationLocator;
        private ISource source;
        private ITarget target;
        private CommandRegistry commandRegistry;
        private DocumentParser documentParser;
        private ISpecificationReader specificationReader;
        private IEvaluatorFactory evaluatorFactory;
        private SpecificationCommand specificationCommand;
        private AssertEqualsCommand assertEqualsCommand;
        private AssertTrueCommand assertTrueCommand;
        private AssertFalseCommand assertFalseCommand;
        private ExecuteCommand executeCommand;
        private RunCommand runCommand;
        private VerifyRowsCommand verifyRowsCommand;
        private EchoCommand echoCommand;
        private string baseOutputDir;
    
        public ConcordionBuilder()
        {
            specificationLocator = new ClassNameBasedSpecificationLocator();
            source = new ClassPathSource();
            target = null;
            commandRegistry = new CommandRegistry();
            documentParser = new DocumentParser(commandRegistry);
            evaluatorFactory = new SimpleEvaluatorFactory();
            specificationCommand = new SpecificationCommand();
            assertEqualsCommand = new AssertEqualsCommand();
            assertTrueCommand = new AssertTrueCommand();
            assertFalseCommand = new AssertFalseCommand();
            executeCommand = new ExecuteCommand();
            runCommand = new RunCommand();
            verifyRowsCommand = new VerifyRowsCommand();
            echoCommand = new EchoCommand();

            //throwableListenerPublisher.addThrowableListener(new ThrowableRenderer());
            
            commandRegistry.Register("", "specification", specificationCommand);
            WithApprovedCommand(NAMESPACE_CONCORDION_2007, "run", runCommand);
            WithApprovedCommand(NAMESPACE_CONCORDION_2007, "execute", executeCommand);
            WithApprovedCommand(NAMESPACE_CONCORDION_2007, "set", new SetCommand());
            WithApprovedCommand(NAMESPACE_CONCORDION_2007, "assertEquals", assertEqualsCommand);
            WithApprovedCommand(NAMESPACE_CONCORDION_2007, "assertTrue", assertTrueCommand);
            WithApprovedCommand(NAMESPACE_CONCORDION_2007, "assertFalse", assertFalseCommand);
            WithApprovedCommand(NAMESPACE_CONCORDION_2007, "verifyRows", verifyRowsCommand);
            WithApprovedCommand(NAMESPACE_CONCORDION_2007, "echo", echoCommand);

            string stylesheetContent = HtmlFramework.EMBEDDED_STYLESHEET_RESOURCE;
            
            //assertEqualsCommand.addAssertEqualsListener(new AssertEqualsResultRenderer());
            //assertTrueCommand.addAssertEqualsListener(new AssertEqualsResultRenderer());
            //assertFalseCommand.addAssertEqualsListener(new AssertEqualsResultRenderer());
            //verifyRowsCommand.addVerifyRowsListener(new VerifyRowsResultRenderer());
            //runCommand.addRunListener(new RunResultRenderer());
            //documentParser.addDocumentParsingListener(new DocumentStructureImprover());
            //documentParser.addDocumentParsingListener(new StylesheetEmbedder(stylesheetContent));
        }
    
        public ConcordionBuilder WithSource(ISource source) 
        {
            this.source = source;
            return this;
        }

        public ConcordionBuilder WithTarget(ITarget target) 
        {
            this.target = target;
            return this;
        }

        public ConcordionBuilder WithEvaluatorFactory(IEvaluatorFactory evaluatorFactory) 
        {
            this.evaluatorFactory = evaluatorFactory;
            return this;
        }
        
        //public ConcordionBuilder withThrowableListener(ThrowableCaughtListener throwableListener) 
        //{
        //    throwableListenerPublisher.addThrowableListener(throwableListener);
        //    return this;
        //}

        //public ConcordionBuilder withAssertEqualsListener(AssertEqualsListener listener) 
        //{
        //    assertEqualsCommand.addAssertEqualsListener(listener);
        //    return this;
        //}

        private ConcordionBuilder WithApprovedCommand(string namespaceURI, string commandName, ICommand command) 
        {
            ExceptionCatchingDecorator throwableCatchingDecorator = new ExceptionCatchingDecorator(new LocalTextDecorator(command));
            //throwableCatchingDecorator.addThrowableListener(throwableListenerPublisher);
            ICommand decoratedCommand = throwableCatchingDecorator;
            commandRegistry.Register(namespaceURI, commandName, decoratedCommand);
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
        
        public Concordion Build() 
        {
            //if (target == null) 
            //{
            //    target = new FileTarget(BaseOutputDir);
            //}
            
            //specificationCommand.addSpecificationListener(new BreadcrumbRenderer(source, xmlParser));
            //specificationCommand.addSpecificationListener(new PageFooterRenderer(target));
            //specificationCommand.addSpecificationListener(new SpecificationExporter(target));

            specificationReader = new XmlSpecificationReader(source, documentParser);        

            return new Concordion(specificationLocator, specificationReader, evaluatorFactory);
        }

        private string BaseOutputDir
        {
            get
            {
                if (baseOutputDir != null)
                {
                    return baseOutputDir;
                }

                // TODO - should parse values from the app.config file here to determine directories to output to
                //string outputPath = System.getProperty(PROPERTY_OUTPUT_DIR);
                string outputPath = @"C:\temp";

                if (String.IsNullOrEmpty(outputPath))
                {
                    //return new File(System.getProperty("java.io.tmpdir"), "concordion");
                    return Path.GetFullPath(outputPath);
                }

                return Path.GetFullPath(outputPath);
            }
        }
    }
}
