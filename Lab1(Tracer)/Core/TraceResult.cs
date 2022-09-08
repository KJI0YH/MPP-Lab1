namespace Tracer.Core
{
    public class TraceResult
    {
        public IReadOnlyList<ThreadTrace> Threads { get; }

        public TraceResult(IReadOnlyList<ThreadTrace> threads)
        {
            Threads = threads;
        }
    }
}
