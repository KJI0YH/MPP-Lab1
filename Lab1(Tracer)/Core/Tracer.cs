using System.Diagnostics;

namespace Lab1_Tracer_.Core
{
    public class Tracer : ITracer
    {
        private readonly TraceResult _traceResult;

        public Tracer()
        {
            _traceResult = new TraceResult();
        }

        public void StartTrace()
        {
            StackTrace stackTrace = new StackTrace();

            var method = stackTrace.GetFrame(1).GetMethod();
            string methodName = method.Name;
            string className = method.DeclaringType.Name;

            ThreadTrace threadTrace = _traceResult.GetThreadTrace(Thread.CurrentThread.ManagedThreadId);
            threadTrace.AddMethod(methodName, className);
        }

        public void StopTrace()
        {
            StackTrace stackTrace = new StackTrace();

            var method = stackTrace.GetFrame(1).GetMethod();
            string methodName = method.Name;
            string className = method.DeclaringType.Name;

            ThreadTrace threadTrace = _traceResult.GetThreadTrace(Thread.CurrentThread.ManagedThreadId);
            threadTrace.StopMethod(methodName, className);
        }

        public TraceResult GetTraceResult()
        {
            return _traceResult;
        }
    }
}
