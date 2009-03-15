using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal
{
    public class CommandCallList : IList<CommandCall>
    {
        #region Properties

        private List<CommandCall> CommandCalls
        {
            get;
            set;
        }

        public bool IsEmpty
        {
            get
            {
                return CommandCalls.Count == 0;
            }
        }

        #endregion

        #region Constructors

        public CommandCallList()
        {
            CommandCalls = new List<CommandCall>();
        }

        #endregion

        #region Methods

        public void ProcessSequentially(IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            foreach (CommandCall commandCall in CommandCalls)
            {
                commandCall.SetUp(evaluator, resultRecorder);
                commandCall.Execute(evaluator, resultRecorder);
                commandCall.Verify(evaluator, resultRecorder);
            }
        }

        public void SetUp(IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            foreach (CommandCall commandCall in CommandCalls)
            {
                commandCall.SetUp(evaluator, resultRecorder);
            }
        }

        public void Execute(IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            foreach (CommandCall commandCall in CommandCalls)
            {
                commandCall.Execute(evaluator, resultRecorder);
            }
        }

        public void Verify(IEvaluator evaluator, IResultRecorder resultRecorder)
        {
            foreach (CommandCall commandCall in CommandCalls)
            {
                commandCall.Verify(evaluator, resultRecorder);
            }
        }

        #endregion

        #region IList<CommandCall> Members

        public int IndexOf(CommandCall item)
        {
            return CommandCalls.IndexOf(item);
        }

        public void Insert(int index, CommandCall item)
        {
            CommandCalls.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            CommandCalls.RemoveAt(index);
        }

        public CommandCall this[int index]
        {
            get
            {
                return CommandCalls[index];
            }
            set
            {
                CommandCalls[index] = value;
            }
        }

        #endregion

        #region ICollection<CommandCall> Members

        public void Add(CommandCall item)
        {
            CommandCalls.Add(item);
        }

        public void Clear()
        {
            CommandCalls.Clear();
        }

        public bool Contains(CommandCall item)
        {
            return CommandCalls.Contains(item);
        }

        public void CopyTo(CommandCall[] array, int arrayIndex)
        {
            CommandCalls.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return CommandCalls.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(CommandCall item)
        {
            return CommandCalls.Remove(item);
        }

        #endregion

        #region IEnumerable<CommandCall> Members

        public IEnumerator<CommandCall> GetEnumerator()
        {
            return CommandCalls.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return CommandCalls.GetEnumerator();
        }

        #endregion
    }
}
