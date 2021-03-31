using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ArdosModel
{
    public class ArDOSItem : IArDOSItem
    {
        public string Text { get; private set; }
        public Color Color { get; private set; }
        public Font Font { get; private set; }
        public Image Image { get; private set; }
        public int Length { get; private set; }
        public bool Dropdown { get; private set; }
        public ArDOSItem Alternative { get; private set; }
        public bool Emojize { get; private set; }
        public bool Ansi { get; private set; }
        public bool UseMarkup { get; private set; }
        public bool Unescape { get; private set; }
        public IAction[] Actions { get; private set; }

        /// <summary>
        /// A representation of a single ArDOS item.
        /// </summary>
        /// <param name="text">The text to display.</param>
        /// <param name="color">The color of the text.</param>
        /// <param name="font">The font of the text.</param>
        /// <param name="fontSize">The font size for the text. If a font is provided, the fontSize will be ignored.</param>
        /// <param name="image">The image to view.</param>
        /// <param name="length">The length at which to truncate the text. 0 means no truncation.</param>
        /// <param name="trim">Whether to trim whitespaces before the text.</param>
        /// <param name="dropdown">Whether the item is part of dropdown. Applies only to last items.</param>
        /// <param name="alternative">The alternative item to view.</param>
        /// <param name="emojize">Whether to use emojis.</param>
        /// <param name="ansi">Whether to interpert ANSI escape sequance.</param>
        /// <param name="useMarkup">Whether to use Pongo markup.</param>
        /// <param name="unescape">Whether to escape characters.</param>
        /// <param name="actions">The actions to run when clicking on the item.</param>
        public ArDOSItem(string text, Color color = new Color(), Font font = null, float fontSize = 12.0f,
            Image image = null, int length = 0, bool trim = true, bool dropdown = true,
            ArDOSItem alternative = null, bool emojize = true, bool ansi = true, bool useMarkup = true,
            bool unescape = true, IAction[] actions = null)
        {
            this.Text = trim ? text.Trim() : text;
            this.Color = (color == Color.Empty) ? Color.Black : color;
            this.Font = font ?? new Font("Ariel", fontSize);
            this.Image = image;
            this.Length = length;
            this.Dropdown = dropdown;
            this.Alternative = alternative;
            this.Emojize = emojize;
            this.Ansi = ansi;
            this.UseMarkup = useMarkup;
            this.Unescape = unescape;
            this.Actions = actions ?? Array.Empty<IAction>();
        }
    }
}
