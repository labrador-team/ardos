using System;
using System.Linq;

namespace ArDOS.Model.Actions
{
    public abstract class BashAction : IAction
    {
        public string ActionType => "CMD";

        private string Command { get; set; }
        private string[] Params { get; set; }
        public bool Terminal { get; set; }

        /// <summary>
        /// The full formatted command as a string.
        /// </summary>
        public string FullCommand => 
                Params.Aggregate(Command, (current, item) => $"{current} {item}");

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
        public abstract void Run();
    }
}
