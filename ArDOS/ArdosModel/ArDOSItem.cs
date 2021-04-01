using System;
using System.Drawing;

namespace ArDOS.Model
{
    /// <summary>
    /// A representation of a single ArDOS item
    /// </summary>
    public class ArdosItem
    {
        /// <summary>
        /// The text to display
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The color of the text
        /// </summary>
        public Color Color { get; set; } = Color.Black;

        /// <summary>
        /// The font of the text
        /// </summary>
        public Font Font { get; set; } = new Font("Segoe UI", 9);

        /// <summary>
        /// The image to view
        /// </summary>
        public Image Image { get; set; } = null;

        /// <summary>
        /// The length at which to truncate the text. 0 means no truncation.
        /// </summary>
        public int Length { get; set; } = 0;

        /// <summary>
        /// Whether the item is part of dropdown. Applies only to last items.
        /// </summary>
        public bool Dropdown { get; set; } = true;

        /// <summary>
        /// The alternative item to view
        /// </summary>
        public ArdosItem AlternativeItem { get; set; } = null;

        /// <summary>
        /// Whether this is an alternative item
        /// </summary>
        public bool Alternate { get; set; } = false;

        /// <summary>
        /// Whether to use emojis
        /// </summary>
        public bool Emojize { get; set; } = true;

        /// <summary>
        /// Whether to interpert ANSI escape sequence
        /// </summary>
        public bool Ansi { get; set; } = true;

        /// <summary>
        /// Whether to use Pongo markup
        /// </summary>
        public bool UseMarkup { get; set; } = true;

        /// <summary>
        /// Whether to escape characters
        /// </summary>
        public bool Unescape { get; set; } = true;

        /// <summary>
        /// Whether to rerun the plugin when this item is clicked
        /// </summary>
        public bool Refresh { get; set; } = false;

        /// <summary>
        /// The actions to run when clicking on the item
        /// </summary>
        public IAction[] Actions { get; set; } = Array.Empty<IAction>();

        /// <summary>
        /// Create a new ArdosItem
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="trim">Whether to trim whitespaces before the text</param>
        public ArdosItem(string text, bool trim = true)
        {
            this.Text = trim ? text.Trim() : text;
        }
    }
}
