using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Internal.Renderer;
using Concordion.Internal.Commands;

namespace Concordion.Spec
{
    class EventRecorder : IAssertEqualsListener, IExceptionListener
    {
        private List<EventArgs> events;

        public EventRecorder()
	    {
            events = new List<EventArgs>();
	    }

        public T GetLast<T>() where T : EventArgs
        {
            T lastMatch = default(T);
            foreach (EventArgs theEvent in events)
            {
                if (theEvent is T)
                {
                    lastMatch = theEvent as T;
                }
            }
            return lastMatch;
        }

        public void ExceptionCaughtEventHandler(object sender, ExceptionCaughtEventArgs e)
        {
            events.Add(e);
        }

        #region IAssertEqualsListener Members

        public void SuccessReportedEventHandler(object sender, SuccessReportedEventArgs e)
        {
            events.Add(e);
        }

        public void FailureReportedEventHandler(object sender, FailureReportedEventArgs e)
        {
            events.Add(e);
        }

        #endregion
    }
}
