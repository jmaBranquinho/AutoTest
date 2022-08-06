using System.Collections;

namespace AutoTest.CodeInterpreter.Models.Wrappers
{
    public class ExecutionPath : IList<StatementWrapper>
    {
        protected List<StatementWrapper> _executionPath = new();

        public StatementWrapper this[int index] { get => _executionPath[index]; set => _executionPath[index] = value; }

        public int Count => _executionPath.Count;

        public bool IsReadOnly => false;

        public ExecutionPath() { }

        public ExecutionPath(IEnumerable<StatementWrapper> items) => AddRange(items);

        public void Add(StatementWrapper item) => _executionPath.Add(item);

        public ExecutionPath AddRange(IEnumerable<StatementWrapper> items)
        {
            items.ToList().ForEach(x => _executionPath.Add(x));
            return this;
        }

        public void Clear() => _executionPath.Clear();

        public bool Contains(StatementWrapper item) => _executionPath.Contains(item);

        public void CopyTo(StatementWrapper[] array, int arrayIndex) => _executionPath.CopyTo(array, arrayIndex);

        public IEnumerator<StatementWrapper> GetEnumerator() => _executionPath.GetEnumerator();

        public int IndexOf(StatementWrapper item) => _executionPath.IndexOf(item);

        public void Insert(int index, StatementWrapper item) => _executionPath.Insert(index, item);

        public bool Remove(StatementWrapper item) => _executionPath.Remove(item);

        public void RemoveAt(int index) => _executionPath.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => _executionPath.GetEnumerator();

        public ExecutionPath Clone()
        {
            var clone = new ExecutionPath();
            clone.AddRange(_executionPath);
            return clone;
        }
    }
}
