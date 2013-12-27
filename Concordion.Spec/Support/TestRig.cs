using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Api.Extension;
using Concordion.Internal;
using Concordion.Internal.Runner;

namespace Concordion.Spec
{
    public class TestRig
    {
        private IConcordionExtension extension;

        private StubTarget m_StubTarget;

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

        public TestRig()
        {
            EvaluatorFactory = new SimpleEvaluatorFactory();
            Source = new StubSource();
        }

        public TestRig WithFixture(object fixture)
        {
            Fixture = fixture;
            return this;
        }

        public ProcessingResult Process(Resource resource)
        {
            var eventRecorder = new EventRecorder();
            this.m_StubTarget = new StubTarget();

            var concordionBuilder = new ConcordionBuilder()
                .WithAssertEqualsListener(eventRecorder)
                .WithExceptionListener(eventRecorder)
                .WithSource(Source)
                .WithEvaluatorFactory(EvaluatorFactory)
                .WithTarget(this.m_StubTarget);

            if (extension != null)
            {
                extension.AddTo(concordionBuilder);
            }

            var concordion = concordionBuilder.Build();

            try
            {
                IResultSummary resultSummary = concordion.Process(resource, Fixture);
                string xml = this.m_StubTarget.GetWrittenString(resource);
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
            return m_StubTarget.HasCopiedResource(resource);
        }

        public TestRig WithExtension(IConcordionExtension extension)
        {
            this.extension = extension;
            return this;
        }
    }
}
