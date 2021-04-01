using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ArDOS.Parser
{
    public partial class WindowsParser
    {
        public static ReadOnlyDictionary<string, string> CONTROL_CHARS = new(new Dictionary<string, string>()
        {
            { "\\'", "\'" },
            { "\\\"", "\"" },
            { "\\\\", "\\" },
            { "\\n", "\n" },
            { "\\r", "\r" },
            { "\\t", "\t" },
            { "\\b", "\b" },
            { "\\0", "\0" }
        });
    }
}
