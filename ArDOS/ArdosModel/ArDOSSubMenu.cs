using System;

namespace ArDOS.Model
{
    public class ArdosSubMenu : ArdosItem
    {
        public ArdosItem[] MenuItems { get; private set; }

        /// <summary>
        /// A representation of an ArDOS sub-menu.
        /// </summary>
        /// <param name="item">The item to display as the menu.</param>
        /// <param name="menuItems">The items of the menu.</param>
        public ArdosSubMenu(string text, bool trim = true, ArdosItem[] menuItems = null) : base(text, trim)
        {
            MenuItems = menuItems ?? Array.Empty<ArdosItem>();
        }
    }
}
