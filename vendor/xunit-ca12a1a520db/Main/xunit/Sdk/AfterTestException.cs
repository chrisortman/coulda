using System;
using System.Collections.Generic;
using System.Text;

namespace Xunit.Sdk
{
    /// <summary>
    /// Exception that is thrown when one or more exceptions are thrown from
    /// the After method of a <see cref="BeforeAfterTestAttribute"/>.
    /// </summary>
    public class AfterTestException : Exception
    {
        List<Exception> afterExceptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="AfterTestException"/> class.
        /// </summary>
        /// <param name="exceptions">The exceptions.</param>
        public AfterTestException(IEnumerable<Exception> exceptions)
        {
            afterExceptions = new List<Exception>(exceptions);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AfterTestException"/> class.
        /// </summary>
        /// <param name="exceptions">The exceptions.</param>
        public AfterTestException(params Exception[] exceptions)
        {
            afterExceptions = new List<Exception>(exceptions);
        }

        /// <summary>
        /// Gets the list of exceptions thrown in the After method.
        /// </summary>
        public List<Exception> AfterExceptions
        {
            get { return afterExceptions; }
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message
        {
            get { return "One or more exceptions were thrown from After methods during test cleanup"; }
        }

        /// <summary>
        /// Gets a string representation of the frames on the call stack at the time the current exception was thrown.
        /// </summary>
        public override string StackTrace
        {
            get
            {
                StringBuilder result = new StringBuilder();

                foreach (Exception ex in AfterExceptions)
                {
                    if (result.Length != 0)
                        result.Append("\r\n\r\n");

                    result.AppendFormat("{0} thrown: {1}\r\n{2}", ex.GetType().FullName, ex.Message, ex.StackTrace);
                }

                return result.ToString();
            }
        }
    }
}
