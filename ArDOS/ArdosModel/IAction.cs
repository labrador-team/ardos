using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdosModel
{
    public interface IAction
    {
        /// <summary>
        /// The action type.
        /// </summary>
        public string ActionType { get; }

        /// <summary>
        /// Run the action.
        /// </summary>
        public void Run();
    }
}
