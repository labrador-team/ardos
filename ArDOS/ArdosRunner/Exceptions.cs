using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArDOS.Runner.Exceptions
{
    public class BaseRunnerException : Exception
    {
        public BaseRunnerException(string message = null, Exception innerException = null) : base(message, innerException) { }
    }

    public class EmptyOutputException : BaseRunnerException
    {
        public EmptyOutputException(string pluginName, Exception innerException = null) : base($"Runner output from plugin {pluginName} returned empty", innerException) { }
    }
}
