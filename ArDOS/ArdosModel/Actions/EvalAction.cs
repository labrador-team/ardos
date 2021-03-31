namespace ArDOS.Model.Actions
{
    public abstract class EvalAction : IAction
    {
        public string ActionType => "EVAL";

        public string Code { get; set; }

        /// <summary>
        /// Create an EvalAction instance
        /// </summary>
        /// <param name="code">A JavaScript code snippet to run</param>
        public EvalAction(string code)
        {
            this.Code = code;
        }

        /// <summary>
        /// Run JavaScript code
        /// </summary>
        public abstract void Run();
    }
}
