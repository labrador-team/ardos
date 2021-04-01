using System;

namespace ArDOS.Model.Actions
{
    /// <summary>
    /// Open an href.
    /// </summary>
    public abstract class HrefAction : IAction
    {
        public string ActionType => "HREF";

        public Uri URI { get; set; }

        /// <summary>
        /// Creates an instance of an HrefAction.
        /// </summary>
        /// <param name="uri">The href to open.</param>
        public HrefAction(Uri uri)
        {
            this.URI = uri;
        }

        public abstract void Run();
    }
}
