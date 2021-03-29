using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdosRunner
{
    public class Scheduler : IDisposable
    {
        public bool Active { get; private set; }
        private readonly CancellationTokenSource _cancellationTokenSource;
        private CancellationToken CancellationToken { get { return this._cancellationTokenSource.Token; } }
        private readonly List<Task> _tasks;

        public Scheduler()
        {
            this.Active = true;
            this._cancellationTokenSource = new CancellationTokenSource();
            this._tasks = new List<Task>();
        }

        public async void Run(string path)
        {
            var task = Runner.GetOutput(path);
            this._tasks.Add(task);
            await task;
        }

        public void Schedule(string path, TimeSpan interval)
        {
            var task = Task.Run(() => this.RunPeriodically(path, interval), this.CancellationToken);
            this._tasks.Add(task);
        }

        private async void RunPeriodically(string path, TimeSpan interval)
        {
            while (this.Active)
            {
                await Runner.GetOutput(path);
                await Task.Delay(interval);
            }
        }

        public void Deactivate()
        {
            this.Active = false;
            this._cancellationTokenSource.Cancel();
            Task.WaitAll(this._tasks.ToArray());
            foreach (var task in this._tasks)
                task.Dispose();
            this._tasks.Clear();
        }

        public void Dispose()
        {
            this.Deactivate();
            this._cancellationTokenSource.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
