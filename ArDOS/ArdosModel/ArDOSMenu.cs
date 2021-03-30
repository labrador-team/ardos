using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ArdosModel
{
    public class ArDOSMenu
    {
        public string Name { get; private set; }
        public Image Icon { get; private set; }
        public IArDOSItem[] MenuItems { get; private set; }
        
        /// <summary>
        /// A repesentation of an ArDOS main menu.
        /// </summary>
        /// <param name="name">The name of the menu.</param>
        /// <param name="icon">The icon to display for the menu.</param>
        /// <param name="menuItems">The items of the menu.</param>
        public ArDOSMenu(string name, Image icon, IArDOSItem[] menuItems = null)
        {
            Name = name;
            Icon = icon;

            menuItems ??= Array.Empty<IArDOSItem>();
            MenuItems = menuItems;
        }
    }
}
