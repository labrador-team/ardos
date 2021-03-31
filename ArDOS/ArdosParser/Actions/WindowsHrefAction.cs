using System;
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
            throw new NotImplementedException();
        }
    }
}
