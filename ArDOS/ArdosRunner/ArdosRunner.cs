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
    public class RunnerOutputEventArgs : EventArgs
    {
        public string Output { get; set; }
        public string Path { get; set; }
    }

    public static class ArdosRunner
    {
        public static event EventHandler<RunnerOutputEventArgs> OnOutputReady;

        public static Task Run(string path)
        {
            return Task.Run(async () =>
            {
                string executable = null;
                using (var reader = new StreamReader(new FileStream(path, FileMode.Open)))
                {
                    char[] buffer = new char[2];
                    await reader.ReadAsync(buffer, 0, 2);
                    if (new string(buffer).Equals("#!"))
                        executable = await reader.ReadLineAsync();
                }
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
                OnOutputReady(null, new RunnerOutputEventArgs { Path = path, Output = output });
            });
        }
    }
}
