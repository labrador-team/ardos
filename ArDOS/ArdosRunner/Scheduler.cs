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
        private Runner _runner;
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken { get { return this._cancellationTokenSource.Token; } }
        private List<Task> _tasks;

        public Scheduler(Runner runner)
        {
            this.Active = true;
            this._runner = runner;
            this._cancellationTokenSource = new CancellationTokenSource();
            this._tasks = new List<Task>();
        }

        public void Run(string path)
        {
            var task = this._runner.Run(path);
            task.Start();
            this._tasks.Add(task);
        }

        public void Schedule(string path, TimeSpan interval)
        {
            var task = Task.Run(() => this.RunPeriodically(path, interval), this._cancellationToken);
            this._tasks.Add(task);
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

        private async void RunPeriodically(string path, TimeSpan interval)
        {
            while (this.Active)
            {
                await this._runner.Run(path);
                await Task.Delay(interval);
            }
        }

        public void Dispose()
        {
            this.Deactivate();
            this._cancellationTokenSource.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
