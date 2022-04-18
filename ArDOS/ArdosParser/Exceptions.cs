using System;

namespace ArDOS.Parser.Exceptions
{
    public class BaseParserException : Exception
    {
        public BaseParserException(string message = null, Exception innerException = null) : base(message, innerException) { }
    }

    public class ParsingException : BaseParserException
    {
        public ParsingException(int lineNumber, string message, Exception innerException = null) : base($"Parsing error on line {lineNumber}: {message}", innerException) { }
    }
}
