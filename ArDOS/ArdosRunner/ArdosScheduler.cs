using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdosRunner
{
    public class ArdosScheduler : IDisposable
    {
        public bool Active { get; private set; }
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken { get { return this._cancellationTokenSource.Token; } }
        private List<Task> _tasks;

        public ArdosScheduler()
        {
            this.Active = true;
            this._cancellationTokenSource = new CancellationTokenSource();
            this._tasks = new List<Task>();
        }

        public async void Run(string path)
        {
            var task = ArdosRunner.Run(path);
            this._tasks.Add(task);
            await task;
        }

        public void Schedule(string path, TimeSpan interval)
        {
            var task = Task.Run(() => this.RunPeriodically(path, interval), this._cancellationToken);
            this._tasks.Add(task);
        }

        private async void RunPeriodically(string path, TimeSpan interval)
        {
            while (this.Active)
            {
                await ArdosRunner.Run(path);
                await Task.Delay(interval);
            }
        }

        public void Deactivate()
        {
            this.Active = false;
            this._cancellationTokenSource.Cancel();
            Task.WaitAll(this._tasks.ToArray(), this._cancellationToken);
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
