using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ArDOS.UI
{
    public partial class MainForm : Form
    {
        public Dictionary<string, NotifyIcon> Plugins { get; protected set; }

        public MainForm()
        {
            InitializeComponent();
            this.Plugins = new Dictionary<string, NotifyIcon>();
            this.fileSystemWatcher.Path = Properties.Settings.Default.ExtensionsDirectoryPath;
            this.StartScheduler();
        }

        protected void SchedulePluginsToRun()
        {
            var info = new DirectoryInfo(Properties.Settings.Default.ExtensionsDirectoryPath);
            var files = info.GetFiles();
            foreach (var fileInfo in files)
            {
                var parts = fileInfo.Name.Split('.', 4);
                string intervalString;
                if (parts.Length == 4) intervalString = parts[2];
                else if (parts.Length == 3) intervalString = parts[1];
                else continue;

                if (intervalString.Equals(""))
                {
                    // Scheduler.Run(fileInfo.FullName);
                }
                else
                {
                    bool runOnOpen;
                    if (runOnOpen = intervalString[^1] == '+')
                        intervalString = intervalString[0..^1];
                    if (!int.TryParse(intervalString[0..^1], out int intervalInt)) continue;
                    TimeSpan interval;
                    switch (intervalString[^1])
                    {
                        case 's':
                            interval = TimeSpan.FromSeconds(intervalInt);
                            break;
                        case 'm':
                            interval = TimeSpan.FromMinutes(intervalInt);
                            break;
                        case 'h':
                            interval = TimeSpan.FromHours(intervalInt);
                            break;
                        case 'd':
                            interval = TimeSpan.FromDays(intervalInt);
                            break;
                    }

                    // Scheduler.Schedule(fileInfo.FullName, interval);
                }
            }
        }

        protected void StartScheduler()
        {

            SchedulePluginsToRun();
        }

        protected void StopScheduler()
        {
            // Close all notify icons
            foreach (var pair in this.Plugins) pair.Value.Dispose();
            this.Plugins.Clear();


        }

        protected void RestartScheduler()
        {
            this.StopScheduler();
            this.StartScheduler();
        }

        protected void Runner_OnOutputReady(object sender, RunnerOutputEventArgs e)
        {
            if (!this.Plugins.TryGetValue(e.Path, out NotifyIcon plugin))
                plugin = new NotifyIcon();
            var contextMenuStrip = new ContextMenuStrip();
            // var menu = Parser.ParsePlugin(e.Output);
            // Create a new context menu strip
            plugin.ContextMenuStrip = contextMenuStrip;
            this.Plugins[e.Path] = plugin;
        }

        private void toolStripMenuItemRefresh_Click(object sender, EventArgs e)
        {
            var info = new DirectoryInfo(Properties.Settings.Default.ExtensionsDirectoryPath);
            var files = info.GetFiles();
            foreach (var fileInfo in files) ;
            // Scheduler.Run(fileInfo.FullName);
        }

        #region Winforms event subcribers
        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            new ConfigForm().ShowDialog();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void toolStripMenuItemQuit_Click(object sender, EventArgs e)
        {
            this.StopScheduler();
            this.Close();
        }

        private void fileSystemWatcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            this.RestartScheduler();
        }

        private void fileSystemWatcher_Error(object sender, System.IO.ErrorEventArgs e)
        {
            // Some error code
        }

        private void fileSystemWatcher_Renamed(object sender, System.IO.RenamedEventArgs e)
        {
            this.RestartScheduler();
        }

        #endregion
    }
}
