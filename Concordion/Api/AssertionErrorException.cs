using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Concordion.Api
{
    /// <summary>
    /// Signals that a specification has not processed properly
    /// </summary>
    public class AssertionErrorException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertionErrorException"/> class.
        /// </summary>
        public AssertionErrorException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertionErrorException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AssertionErrorException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertionErrorException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public AssertionErrorException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertionErrorException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        public AssertionErrorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}
