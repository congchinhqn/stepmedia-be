using System;
using Metatrade.Core.ErrorDefinitions;

namespace Metatrade.Core.Exceptions
{
    public class AppException : DomainException
    {
        #region Constructors

        public AppException()
        {
        }

        public AppException(string message) : base(message)
        {
        }

        public AppException(string message, string code) : base(message, code)
        {
        }

        public AppException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AppException(string message, string code, Exception innerException) : base(message, code, innerException)
        {
        }

        public AppException(Error error) : this(error.Message, error.Code)
        {
        }

        public AppException(Error error, Exception innerException) : this(error.Message, error.Code, innerException)
        {
        }

        #endregion
    }
}