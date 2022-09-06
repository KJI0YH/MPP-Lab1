namespace Tracer.Core
{
    public class TraceResult
    {
        public IReadOnlyDictionary<int, ThreadTrace> Threads { get; }

        public TraceResult(IReadOnlyDictionary<int, ThreadTrace> threads)
        {
            Threads = threads;
        }
    }
}
