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

            StackFrame[] frames = stackTrace.GetFrames();
            List<string> framePath = new List<string>();

            for (int i = frames.Length - 1; i > 0; i--)
            {
                framePath.Add(frames[i].GetMethod().Name);
            }

            var method = stackTrace.GetFrame(1).GetMethod();
            string methodName = method.Name;
            string className = method.DeclaringType.Name;

            ThreadTrace threadTrace = _traceResult.GetThreadTrace(Thread.CurrentThread.ManagedThreadId);
            threadTrace.AddMethod(methodName, className, framePath);
        }

        public void StopTrace()
        {
            StackTrace stackTrace = new StackTrace();

            StackFrame[] frames = stackTrace.GetFrames();
            List<string> framePath = new List<string>();

            for (int i = frames.Length - 1; i > 0; i--)
            {
                framePath.Add(frames[i].GetMethod().Name);
            }

            var method = stackTrace.GetFrame(1).GetMethod();
            string methodName = method.Name;
            string className = method.DeclaringType.Name;

            ThreadTrace threadTrace = _traceResult.GetThreadTrace(Thread.CurrentThread.ManagedThreadId);
            threadTrace.StopMethod(methodName, className, framePath);
        }

        public TraceResult GetTraceResult()
        {
            return _traceResult;
        }
    }
}
