namespace ArDOS.Model
{
    public interface IAction
    {
        /// <summary>
        /// The action type.
        /// </summary>
        public string ActionType { get; }

        /// <summary>
        /// Run the action.
        /// </summary>
        public void Run();
    }
}
