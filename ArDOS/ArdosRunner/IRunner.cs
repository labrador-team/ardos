using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArDOS.Runner
{
    public interface IRunner
    {
        public event EventHandler<RunnerOutputEventArgs> OnOutputReady;
        public Task GetOutput(string path);
    }
}
