using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ArDOS.Model;
using ArDOS.Parser;
using ArDOS.Runner;
using ArDOS.Parser.Exceptions;

namespace ArDOS.UI
{
    public partial class MainForm : Form
    {
        public Dictionary<string, NotifyIcon> Plugins { get; protected set; }

        public IScheduler Scheduler { get; set; }

        public bool Running { get; set; } = false;

        public MainForm()
        {
            InitializeComponent();
            this.Plugins = new Dictionary<string, NotifyIcon>();
            if (new DirectoryInfo(Properties.Settings.Default.ExtensionsDirectoryPath).Exists)
            {
                this.fileSystemWatcher.Path = Properties.Settings.Default.ExtensionsDirectoryPath;
                this.StartScheduler();
            }
        }

        protected void SchedulePluginsToRun()
        {
            Properties.Settings.Default.Reload();
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
                   this.Scheduler.Run(fileInfo.FullName);
                }
                else
                {
                    bool runOnOpen;
                    if (runOnOpen = intervalString[^1] == '+')
                        intervalString = intervalString[0..^1];
                    if (!int.TryParse(intervalString[0..^1], out int intervalInt)) continue;
                    TimeSpan? interval = null;
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
                    if (!interval.HasValue) continue;

                    this.Scheduler.Schedule(fileInfo.FullName, interval.Value);
                }
            }
        }

        protected void StartScheduler()
        {
            if (!this.Running)
            {
                this.Scheduler = new DefaultScheduler(new DefaultRunner(Encoding.UTF8));
                this.Scheduler.Runner.OnOutputReady += Runner_OnOutputReady;
                SchedulePluginsToRun();
                this.Running = true;
            }
        }

        protected void StopScheduler()
        {
            if (this.Running)
            {
                // Close all notify icons
                foreach (var pair in this.Plugins) pair.Value.Dispose();
                this.Plugins.Clear();

                this.Scheduler.Dispose();
                this.Running = false;
            }
        }

        protected void RestartScheduler()
        {
            this.StopScheduler();
            this.StartScheduler();
        }

        protected void Runner_OnOutputReady(object sender, RunnerOutputEventArgs e)
        {
            Task.Run(() =>
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    ArdosMenu menu;
                    try
                    {
                        menu = WindowsParser.Parse(e.Output);
                    }
                    catch (BaseParserException ex)
                    {
                        MessageBox.Show($"Parser Error: {ex.Message}");
                        return;
                    }
                    
                    if (!this.Plugins.TryGetValue(e.Path, out NotifyIcon plugin))
                        plugin = new NotifyIcon(this.components)
                        {
                            Visible = true,
                            ContextMenuStrip = new ContextMenuStrip(this.components),
                        };

                    // Assign attrs to plugin
                    plugin.Text = menu.Name;
                    plugin.Icon = menu.Icon == null ? this.Icon : menu.Icon.ToIcon();

                    // Populate context menu strip
                    plugin.ContextMenuStrip.Items.Clear();
                    plugin.ContextMenuStrip.Items.AddRange(PopulateSubmenu(menu.MenuItems, e.Path, this.Scheduler));

                    // Assign plugin to dictionary
                    this.Plugins[e.Path] = plugin;
                }));
                Application.Run();
            });
        }

        protected ToolStripItem[] PopulateSubmenu(ArdosItem[][] items, string pluginPath, IScheduler scheduler)
        {
            var toolStripItems = new List<ToolStripItem>();
            foreach (var section in items)
            {
                foreach (var item in section)
                {
                    var toolStripItem = new ToolStripMenuItem(item.Text, item.Image, item.RunItem(pluginPath, scheduler))
                    {
                        Font = item.Font,
                        ForeColor = item.Color
                    };

                    if (item.Emojize)
                        foreach (Match match in Regex.Matches(toolStripItem.Text, @":(?<key>\w+):"))
                            if (WindowsParser.EMOJIS.ContainsKey(match.Groups["key"].Value))
                                toolStripItem.Text = toolStripItem.Text.Replace(match.Value, WindowsParser.EMOJIS[match.Groups["key"].Value]);
                    
                    if (item.Unescape)
                        foreach (var pair in WindowsParser.CONTROL_CHARS)
                            toolStripItem.Text = toolStripItem.Text.Replace(pair.Key, pair.Value);
                    
                    if (item.GetType() == typeof(ArdosSubMenu))
                        toolStripItem.DropDownItems.AddRange(PopulateSubmenu(((ArdosSubMenu)item).MenuItems, pluginPath, scheduler));
                    
                    toolStripItems.Add(toolStripItem);
                }
                toolStripItems.Add(new ToolStripSeparator());
            }
            if (toolStripItems.Count > 0) toolStripItems.RemoveAt(toolStripItems.Count - 1);
            return toolStripItems.ToArray();
        }

        private void toolStripMenuItemRefresh_Click(object sender, EventArgs e)
        {
            var info = new DirectoryInfo(Properties.Settings.Default.ExtensionsDirectoryPath);
            var files = info.GetFiles();
            foreach (var fileInfo in files)
                this.Scheduler.Run(fileInfo.FullName);
        }

        #region Winforms event subcribers
        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            new ConfigForm().ShowDialog();
            Properties.Settings.Default.Reload();
            this.fileSystemWatcher.Path = Properties.Settings.Default.ExtensionsDirectoryPath;
            RestartScheduler();
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
