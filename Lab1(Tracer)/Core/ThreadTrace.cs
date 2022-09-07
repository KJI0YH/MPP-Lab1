namespace Tracer.Core
{
    public class ThreadTrace
    {
        public int ThreadID { get; private set; }
        public double Time { get; private set; }
        public IReadOnlyList<MethodTrace> Methods { get; }

        public ThreadTrace(int threadID, IReadOnlyList<MethodTrace> methods)
        {
            Methods = methods;
            ThreadID = threadID;
            Time = methods.Sum(method => method.Time.TotalMilliseconds);
        }
    }
}
