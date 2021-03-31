using System;
using System.Drawing;

namespace ArDOS.Model
{
    public class ArdosMenu
    {
        public string Name { get; private set; }
        public Image Icon { get; private set; }
        public ArdosItem[] MenuItems { get; private set; }
        
        /// <summary>
        /// A repesentation of an ArDOS main menu.
        /// </summary>
        /// <param name="name">The name of the menu.</param>
        /// <param name="icon">The icon to display for the menu.</param>
        /// <param name="menuItems">The items of the menu.</param>
        public ArdosMenu(string name, Image icon = null, ArdosItem[] menuItems = null)
        {
            Name = name;
            Icon = icon;
            MenuItems = menuItems ?? Array.Empty<ArdosItem>();
        }
    }
}
