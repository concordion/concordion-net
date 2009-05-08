using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Internal;
using Concordion.Internal.Runner;

namespace Concordion.Spec
{
    class TestRig
    {
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
            var stubTarget = new StubTarget();

            var concordion = new ConcordionBuilder()
                .WithAssertEqualsListener(eventRecorder)
                .WithExceptionListener(eventRecorder)
                .WithSource(Source)
                .WithEvaluatorFactory(EvaluatorFactory)
                .WithTarget(stubTarget)
                .Build();

            try
            {
                IResultSummary resultSummary = concordion.Process(resource, Fixture);
                string xml = stubTarget.GetWrittenString(resource);
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
    }
}
