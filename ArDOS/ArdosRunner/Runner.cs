using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace ArdosRunner
{
    public static class Runner
    {
        public static event EventHandler<RunnerOutputEventArgs> OnOutputReady;

        public static Task GetOutput(string path)
        {
            return Task.Run(() => GetOutputAsync(path));
        }

        private static async void GetOutputAsync(string path)
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
                Arguments = executable.Equals(null) ? null : path,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.UTF8,
                UseShellExecute = false
            };
            string output;
            using var process = Process.Start(startInfo);
            output = await process.StandardOutput.ReadToEndAsync();

            // Trigger the event with the given output
            OnOutputReady(null, new RunnerOutputEventArgs { Path = path, Output = output });
        }
    }
}
