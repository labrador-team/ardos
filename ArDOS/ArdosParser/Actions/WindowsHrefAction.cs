using System;
using System.Diagnostics;
using ArDOS.Model.Actions;

namespace ArDOS.Parser.Actions
{
    public class WindowsHrefAction : HrefAction
    {
        public WindowsHrefAction(Uri uri) : base(uri)
        {
        }

        public override void Run()
        {
            Process.Start("explorer.exe", this.URI.AbsoluteUri);
        }
    }
}
