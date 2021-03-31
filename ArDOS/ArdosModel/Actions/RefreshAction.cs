namespace ArDOS.Model.Actions
{
    public abstract class RefreshAction : IAction
    {
        public string ActionType => "REFRESH";

        /// <summary>
        /// Refresh the menu
        /// </summary>
        public abstract void Run();
    }
}
