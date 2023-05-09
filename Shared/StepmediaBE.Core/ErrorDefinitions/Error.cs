namespace Metatrade.Core.ErrorDefinitions
{
    public class Error
    {
        public string Code { get; }
        public string Message { get; }

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public Error(string code, string message, params object[] parameters) : this(code,
            string.Format(message, parameters))
        {
        }
    }
}