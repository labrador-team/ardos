using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArDOS.Runner
{
    public interface IScheduler : IDisposable
    {
        public bool Active { get; }
        public IRunner Runner { get; }
        public void Run(string path);
        public void Schedule(string path, TimeSpan interval);
        public void Deactivate();
    }
}
