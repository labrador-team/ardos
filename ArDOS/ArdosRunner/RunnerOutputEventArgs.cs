using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdosRunner
{
    public class RunnerOutputEventArgs : EventArgs
    {
        public string Output { get; set; }
        public string Path { get; set; }
    }
}
