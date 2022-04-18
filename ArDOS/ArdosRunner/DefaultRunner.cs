using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using ArDOS.Runner.Exceptions;

namespace ArDOS.Runner
{
    public class DefaultRunner : IRunner
    {
        public const string ARDOS_VERSION = "1.0.0";

        public event EventHandler<RunnerOutputEventArgs> OnOutputReady;
        public Encoding OutputEncoding { get; protected set; }

        public DefaultRunner(Encoding outputEncoding = null)
        {
            this.OutputEncoding = outputEncoding ?? Encoding.UTF8;
        }

        public Task GetOutput(string path)
        {
            return Task.Run(() => GetOutputAsync(path));
        }

        protected async void GetOutputAsync(string path)
        {
            // Find how to run the file, default to cmd "start"
            string executable = null;
            using (var reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read)))
            {
                char[] buffer = new char[2];
                await reader.ReadAsync(buffer, 0, 2);
                if (new string(buffer).Equals("#!"))
                    executable = await reader.ReadLineAsync();
            }

            // Run the file and get the output
            var startInfo = new ProcessStartInfo
            {
                FileName = executable ?? path,
                Arguments = executable == null ? "" : $"\"{path}\"",
                RedirectStandardOutput = true,
                StandardOutputEncoding = this.OutputEncoding,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            startInfo.Environment["ARDOS_VERSION"] = ARDOS_VERSION;
            string output;
            using var process = Process.Start(startInfo);
            output = await process.StandardOutput.ReadToEndAsync();

            if (output == string.Empty) throw new EmptyOutputException(path);

            // Trigger the event with the given output
            OnOutputReady(null, new RunnerOutputEventArgs { Path = path, Output = output });
        }
    }
}
