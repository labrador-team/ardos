using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace ArDOS.UI
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();
            this.textBoxExtensionsDirectoryPath.Text = Properties.Settings.Default.ExtensionsDirectoryPath;
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                textBoxExtensionsDirectoryPath.Text = folderBrowserDialog.SelectedPath;
        }

        private void ConfigForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.ExtensionsDirectoryPath = textBoxExtensionsDirectoryPath.Text;
            Properties.Settings.Default.Save();
        }
    }
}
