using System;
using System.Diagnostics;
using ArDOS.Model.Actions;

namespace ArDOS.Parser.Actions
{
    public class WindowsCMDAction : BashAction
    {
        public WindowsCMDAction(string command, string[] commandParams = null, bool terminal = false): base(command, commandParams, terminal)
        {
        }

        public override void Run()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/{(this.Terminal ? 'k' : 'c')} {this.FullCommand}",
                UseShellExecute = true,
                CreateNoWindow = !this.Terminal,
                WindowStyle = this.Terminal ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden
            };
            Process.Start(startInfo);
        }
    }
}
