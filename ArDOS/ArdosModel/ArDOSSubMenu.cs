using System;

namespace ArDOS.Model
{
    public class ArdosSubMenu : ArdosItem
    {
        public ArdosItem[][] MenuItems { get; private set; }

        /// <summary>
        /// A representation of an ArDOS sub-menu.
        /// </summary>
        /// <param name="item">The item to display as the menu.</param>
        /// <param name="menuItems">The items of the menu.</param>
        public ArdosSubMenu(string text, bool trim = true, ArdosItem[][] menuItems = null) : base(text, trim)
        {
            MenuItems = menuItems ?? Array.Empty<ArdosItem[]>();
        }

        public ArdosSubMenu(ArdosItem item, ArdosItem[][] menuItems = null) : this(item.Text, false, menuItems)
        {
            this.Actions = item.Actions;
            this.AlternativeItem = item.AlternativeItem;
            this.Ansi = item.Ansi;
            this.Color = item.Color;
            this.Dropdown = item.Dropdown;
            this.Emojize = item.Emojize;
            this.Font = item.Font;
            this.Image = item.Image;
            this.Length = item.Length;
            this.Unescape = item.Unescape;
            this.UseMarkup = item.UseMarkup;
        }
    }
}
