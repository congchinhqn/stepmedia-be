using System;
using Metatrade.Core.ErrorDefinitions;

namespace Metatrade.Core.Exceptions
{
    /// <summary>
    ///     Exception type for domain exceptions
    /// </summary>
    public class DomainException : Exception
    {
        #region Constructors

        public DomainException()
        {
        }

        public DomainException(string message)
            : base(message)
        {
        }

        public DomainException(string message, string code)
            : base(message)
        {
            Code = code;
        }

        public DomainException(Error error) : this(error.Message, error.Code)
        {
        }

        public DomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DomainException(string message, string code, Exception innerException)
            : base(message, innerException)
        {
            Code = code;
        }

        #endregion

        #region Properties

        public string Code { get; }

        #endregion
    }
}