using System.Collections.Concurrent;

namespace Lab1_Tracer_.Core
{
    public class TraceResult
    {
        private ConcurrentDictionary<int, ThreadTrace> Threads = new ConcurrentDictionary<int, ThreadTrace>();

        public TraceResult()
        {

        }

        public ThreadTrace GetThreadTrace(int threadID)
        {
            return Threads.GetOrAdd(threadID, new ThreadTrace(threadID));
        }
    }
}
