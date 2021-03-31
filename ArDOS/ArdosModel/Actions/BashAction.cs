using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdosModel
{
    public class BashAction : IAction
    {
        public string ActionType { get; } = "bash".ToUpper();

        private string Command { get; set; }
        private string[] Params { get; set; }
        public bool Terminal { get; private set; }

        /// <summary>
        /// The full formatted command as a string.
        /// </summary>
        public string FullCommand 
        { 
            get 
            {
                return Params.Aggregate(Command, (current, item) => current + " " + item);
            } 
        }

        /// <summary>
        /// Creates a BashAction instance.
        /// </summary>
        /// <param name="command">The command to run. May include params.</param>
        /// <param name="commandParams">The params to pass to the command. Will be appended at the end of the command.</param>
        /// <param name="terminal">Whether or not to open a terminal window.</param>
        public BashAction(string command, string[] commandParams = null, bool terminal = false)
        {
            this.Command = command;
            this.Params = commandParams ?? Array.Empty<string>();
            this.Terminal = terminal;
        }

        /// <summary>
        /// Runs a command.
        /// </summary>
        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
