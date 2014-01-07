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
using Concordion.Api;
using System.Threading;
using Concordion.Api.Listener;

namespace Concordion.Internal.Commands
{
    public class ExceptionCatchingDecorator : AbstractCommandDecorator
    {
        private readonly List<IExceptionCaughtListener> m_Listeners;

        #region Constructors
        
        public ExceptionCatchingDecorator(ICommand command)
            : base(command)
        {
            this.m_Listeners = new List<IExceptionCaughtListener>();
        } 

        #endregion

        #region Methods

        public void AddExceptionListener(IExceptionCaughtListener listener)
        {
            m_Listeners.Add(listener);
        }

        public void RemoveExceptionListener(IExceptionCaughtListener listener)
        {
            m_Listeners.Remove(listener);
        }

        private void AnnounceThrowableCaught(Element element, Exception exception, string expression)
        {
            foreach (var listener in m_Listeners)
            {
                listener.ExceptionCaught(new ExceptionCaughtEvent(exception, element, expression));
            }
        }

        #endregion

        #region Override Methods

        public override void Setup(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            try
            {
                m_command.Setup(commandCall, evaluator, resultRecorder);
            }
            catch (Exception e)
            {
                resultRecorder.Record(Result.Exception);
                AnnounceThrowableCaught(commandCall.Element, e, commandCall.Expression);
            }
        }

        public override void Execute(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            try
            {
                m_command.Execute(commandCall, evaluator, resultRecorder);
            }
            catch (Exception e)
            {
                resultRecorder.Record(Result.Exception);
                AnnounceThrowableCaught(commandCall.Element, e, commandCall.Expression);
            }
        }

        public override void Verify(CommandCall commandCall, IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            try
            {
                m_command.Verify(commandCall, evaluator, resultRecorder);
            }
            catch (Exception e)
            {
                resultRecorder.Record(Result.Exception);
                AnnounceThrowableCaught(commandCall.Element, e, commandCall.Expression);
            }
        }

        #endregion
    }
}
