using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdosModel
{
    public class RefreshAction : IAction
    {
        public string ActionType { get; } = "refresh".ToUpper();

        /// <summary>
        /// Refresh the menu.
        /// </summary>
        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
