using System;
using System.Collections.Generic;
using System.Linq;
using Concordion.Api.Listener;

namespace Concordion.Spec.Support
{
    public class EventRecorder : IAssertEqualsListener, IExceptionCaughtListener
    {
        private readonly List<Object> m_Events;

        public EventRecorder()
	    {
            this.m_Events = new List<Object>();
	    }

        public Object GetLast(Type eventType)
        {
            Object lastMatch = null;
            foreach (var anEvent in this.m_Events.Where(eventType.IsInstanceOfType))
            {
                lastMatch = anEvent;
            }
            return lastMatch;
        }

        public void ExceptionCaught(ExceptionCaughtEvent caughtEvent)
        {
            this.m_Events.Add(caughtEvent);
        }

        #region IAssertEqualsListener Members

        public void SuccessReported(AssertSuccessEvent successEvent)
        {
            this.m_Events.Add(successEvent);
        }

        public void FailureReported(AssertFailureEvent failureEvent)
        {
            this.m_Events.Add(failureEvent);
        }

        #endregion
    }
}
