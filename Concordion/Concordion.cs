using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;
using Concordion.Internal;

namespace Concordion
{
    public class Concordion
    {
        #region Properties
        
        private ISpecificationLocator SpecificationLocator
        {
            get;
            set;
        }

        private IEvaluatorFactory EvaluatorFactory
        {
            get;
            set;
        }

        private ISpecificationReader SpecificationReader
        {
            get;
            set;
        } 

        #endregion

        #region Constructors

        public Concordion(ISpecificationLocator specificationLocator, ISpecificationReader specificationReader, IEvaluatorFactory evaluatorFactory)
        {
            SpecificationLocator = specificationLocator;
            SpecificationReader = specificationReader;
            EvaluatorFactory = evaluatorFactory;
        }

        #endregion

        #region Methods

        public IResultSummary Process(object fixture)
        {
            return Process(SpecificationLocator.LocateSpecification(fixture), fixture);
        }

        public IResultSummary Process(Resource resource, object fixture) 
        {
            var specification = SpecificationReader.ReadSpecification(resource);
            var resultRecorder = new SummarizingResultRecorder();
            specification.Process(EvaluatorFactory.CreateEvaluator(fixture), resultRecorder);
            return resultRecorder;
        }

        #endregion
    }
}
