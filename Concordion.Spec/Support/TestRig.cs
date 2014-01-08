using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Api.Extension;
using Concordion.Internal;
using Concordion.Internal.Extension;
using Concordion.Internal.Runner;

namespace Concordion.Spec
{
    public class TestRig
    {
        public SpecificationConfig Configuration { get; set; }

        private object Fixture
        {
            get;
            set;
        }

        private IEvaluatorFactory EvaluatorFactory
        {
            get;
            set;
        }

        private StubSource Source
        {
            get;
            set;
        }

        private StubTarget Target
        {
            get;
            set;
        }

        private IConcordionExtension Extension { get; set; }

        public TestRig()
        {
            EvaluatorFactory = new SimpleEvaluatorFactory();
            Source = new StubSource();
            Configuration = new SpecificationConfig();
        }

        public TestRig WithFixture(object fixture)
        {
            Fixture = fixture;
            return this;
        }

        public ProcessingResult Process(Resource resource)
        {
            var eventRecorder = new EventRecorder();
            this.Target = new StubTarget();

            var concordionBuilder = new ConcordionBuilder()
                .WithEvaluatorFactory(EvaluatorFactory)
                .WithSource(Source)
                .WithTarget(Target)
                .WithAssertEqualsListener(eventRecorder)
                .WithExceptionListener(eventRecorder);

            if (Fixture != null)
            {
                new ExtensionLoader(Configuration).AddExtensions(Fixture, concordionBuilder);
            }
            
            if (Extension != null)
            {
                Extension.AddTo(concordionBuilder);
            }

            var concordion = concordionBuilder.Build();

            try
            {
                IResultSummary resultSummary = concordion.Process(resource, Fixture);
                string xml = Target.GetWrittenString(resource);
                return new ProcessingResult(resultSummary, eventRecorder, xml);
            }
            catch (Exception e)
            {
                throw new Exception("Test rig failed to process specification", e);
            }
        }

        public ProcessingResult Process(string html)
        {
            Resource resource = new Resource("/testrig");
            WithResource(resource, html);
            return Process(resource);
        }

        public TestRig WithResource(Resource resource, string html)
        {
            Source.AddResource(resource, html);
            return this;
        }

        public TestRig WithStubbedEvaluationResult(object evaluationResult)
        {
            this.EvaluatorFactory = new StubEvaluator(this.Fixture).withStubbedResult(evaluationResult);
            return this;
        }

        public ProcessingResult ProcessFragment(string fragment)
        {
            return Process(WrapFragment(fragment));
        }

        private string WrapFragment(string fragment)
        {
            var wrappedFragment = "<body><fragment>" + fragment + "</fragment></body>";
            return WrapWithNamespaceDeclaration(wrappedFragment);
        }

        private string WrapWithNamespaceDeclaration(string fragment)
        {
            return "<html xmlns:concordion='"
                + HtmlFramework.NAMESPACE_CONCORDION_2007 + "'>"
                + fragment
                + "</html>";
        }

        public bool HasCopiedResource(Resource resource)
        {
            return Target.HasCopiedResource(resource);
        }

        public TestRig WithExtension(IConcordionExtension extension)
        {
            this.Extension = extension;
            return this;
        }
    }
}
