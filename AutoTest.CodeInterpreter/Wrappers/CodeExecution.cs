namespace AutoTest.CodeInterpreter.Wrappers
{
    public class CodeExecution
    {
        public List<StatementWrapper> Execution { get; set; } = new List<StatementWrapper>();

        public bool IsFinished { get; set; }

        public CodeExecution Clone()
        {
            return new CodeExecution
            {
                Execution = new List<StatementWrapper>(Execution),
                IsFinished = IsFinished
            };
        }
    }
}
