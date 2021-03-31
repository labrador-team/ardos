using System;
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
            throw new NotImplementedException();
        }
    }
}
