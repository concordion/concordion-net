using System;
using System.Collections.Generic;
using System.Linq;
using Concordion.Api;
using Concordion.Api.Extension;
using Concordion.Internal;
using Concordion.Internal.Extension;

namespace Concordion.Spec.Support
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
            this.EvaluatorFactory = new SimpleEvaluatorFactory();
            this.Source = new StubSource();
            this.Configuration = new SpecificationConfig();
        }

        public TestRig WithFixture(object fixture)
        {
            this.Fixture = fixture;
            return this;
        }

        public ProcessingResult Process(Resource resource)
        {
            var eventRecorder = new EventRecorder();
            this.Target = new StubTarget();

            var concordionBuilder = new ConcordionBuilder()
                .WithEvaluatorFactory(this.EvaluatorFactory)
                .WithSource(this.Source)
                .WithTarget(this.Target)
                .WithAssertEqualsListener(eventRecorder)
                .WithExceptionListener(eventRecorder);

            if (this.Fixture != null)
            {
                new ExtensionLoader(this.Configuration).AddExtensions(this.Fixture, concordionBuilder);
            }
            
            if (this.Extension != null)
            {
                this.Extension.AddTo(concordionBuilder);
            }

            var concordion = concordionBuilder.Build();

            try
            {
                IResultSummary resultSummary = concordion.Process(resource, this.Fixture);
                string xml = this.Target.GetWrittenString(resource);
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
            this.WithResource(resource, html);
            return this.Process(resource);
        }

        public TestRig WithResource(Resource resource, string html)
        {
            this.Source.AddResource(resource, html);
            return this;
        }

        public TestRig WithStubbedEvaluationResult(object evaluationResult)
        {
            this.EvaluatorFactory = new StubEvaluator(this.Fixture).withStubbedResult(evaluationResult);
            return this;
        }

        public ProcessingResult ProcessFragment(string fragment)
        {
            return this.Process(this.WrapFragment(fragment));
        }

        private string WrapFragment(string fragment)
        {
            var wrappedFragment = "<body><fragment>" + fragment + "</fragment></body>";
            return this.WrapWithNamespaceDeclaration(wrappedFragment);
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
            return this.Target.HasCopiedResource(resource);
        }

        public TestRig WithExtension(IConcordionExtension extension)
        {
            this.Extension = extension;
            return this;
        }
    }
}
