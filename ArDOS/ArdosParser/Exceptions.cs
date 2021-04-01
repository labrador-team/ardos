using System;

namespace ArDOS.Parser.Exceptions
{
    public class ParsingException : FormatException
    {
        public ParsingException(int lineNumber, string message) : base($"Parsing error on line {lineNumber}: {message}") { }
    }
}
