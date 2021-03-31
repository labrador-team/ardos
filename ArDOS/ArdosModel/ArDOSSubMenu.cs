using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdosModel
{
    public class ArDOSSubMenu : IArDOSItem
    {
        public ArDOSItem Item { get; private set; }
        public IArDOSItem[] MenuItems { get; private set; }

        /// <summary>
        /// A representation of an ArDOS sub-menu.
        /// </summary>
        /// <param name="item">The item to display as the menu.</param>
        /// <param name="menuItems">The items of the menu.</param>
        public ArDOSSubMenu(ArDOSItem item, IArDOSItem[] menuItems = null)
        {
            this.Item = item;
            this.MenuItems = menuItems ?? Array.Empty<IArDOSItem>();
        }
    }
}
