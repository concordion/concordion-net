using System.IO;
using Concordion.Api.Listener;

namespace Concordion.Spec.Concordion.Extension.Listener
{
    public class VerifyRowsLogger : IVerifyRowsListener
    {
        #region Fields

        private readonly TextWriter m_LogWriter;

        #endregion

        #region IVerifyRowsListener Members

        public VerifyRowsLogger(TextWriter logWriter)
        {
            this.m_LogWriter = logWriter;
        }

        public void ExpressionEvaluated(ExpressionEvaluatedEvent expressionEvaluatedEvent)
        {
            this.m_LogWriter.WriteLine("Evaluated '{0}'",
                                  expressionEvaluatedEvent.Element.GetAttributeValue("verifyRows", HtmlFramework.NAMESPACE_CONCORDION_2007));
        }

        public void MissingRow(MissingRowEvent missingRowEvent)
        {
            this.m_LogWriter.WriteLine("Missing Row '{0}'", missingRowEvent.RowElement.Text);
        }

        public void SurplusRow(SurplusRowEvent surplusRowEvent)
        {
            this.m_LogWriter.WriteLine("Surplus Row '{0}'", surplusRowEvent.RowElement.Text);
        }

        #endregion

    }
}
