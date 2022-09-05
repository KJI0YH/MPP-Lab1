using System.Diagnostics;
using System.Reflection;

namespace Lab1_Tracer_.Core
{
    public class Tracer : ITracer
    {
        private readonly TraceResult _traceResult;

        public Tracer()
        {
            _traceResult = new TraceResult();
        }

        // Start method tracing
        public void StartTrace()
        {
            StackTrace stackTrace = new StackTrace();

            List<string> framePath = CreateFramePath(stackTrace.GetFrames());

            var method = stackTrace.GetFrame(1).GetMethod();

            ThreadTrace threadTrace = _traceResult.GetThreadTrace(Thread.CurrentThread.ManagedThreadId);
            threadTrace.AddMethod(method.Name, method.DeclaringType.Name, framePath);
        }

        // Stop method tracing
        public void StopTrace()
        {
            StackTrace stackTrace = new StackTrace();

            List<string> framePath = CreateFramePath(stackTrace.GetFrames());

            var method = stackTrace.GetFrame(1).GetMethod();

            ThreadTrace threadTrace = _traceResult.GetThreadTrace(Thread.CurrentThread.ManagedThreadId);
            threadTrace.StopMethod(method.Name, method.DeclaringType.Name, framePath);
        }

        // Get trace result by threads
        public TraceResult GetTraceResult()
        {
            return _traceResult;
        }

        // Creating frame path from methods
        private List<string> CreateFramePath(StackFrame[] frames)
        {
            List<string> framePath = new List<string>();

            for (int i = frames.Length - 1; i > 0; i--)
            {
                MethodBase? methodBase = frames[i].GetMethod();

                if (methodBase != null)
                {
                    framePath.Add(methodBase.Name);
                }
            }
            return framePath;
        }
    }
}
