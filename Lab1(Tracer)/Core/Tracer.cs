using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;

namespace Tracer.Core
{
    public class Tracer : ITracer
    {
        private readonly ConcurrentDictionary<int, ThreadInfo> _threads = new();

        private struct MethodInfo
        {
            public string Name { get; set; }
            public string ClassName { get; set; }
            public List<string> FramePath { get; set; }
            public Stopwatch Stopwatch { get; set; }

            public MethodInfo(string name, string className, List<string> framePath, Stopwatch stopwatch)
            {
                Name = name;
                ClassName = className;
                FramePath = framePath;
                Stopwatch = stopwatch;
            }
        }

        private struct ThreadInfo
        {
            public ConcurrentStack<MethodInfo> RunningMethods { get; set; }
            public List<MethodTrace> Methods { get; set; }

            public ThreadInfo()
            {
                RunningMethods = new ConcurrentStack<MethodInfo>();
                Methods = new List<MethodTrace>();
            }
        }

        // Start method tracing
        public void StartTrace()
        {
            StackTrace stackTrace = new StackTrace();

            // Get method to measure
            MethodBase? method = null;
            StackFrame? frame = stackTrace.GetFrame(1);
            if (frame != null)
            {
                method = frame.GetMethod();
            }

            if (method != null)
            {
                StackFrame[] stackFrames = stackTrace.GetFrames();
                List<string> framePath = CreateFramePath(stackFrames);
                string className = method.DeclaringType == null ? string.Empty : method.DeclaringType.Name;
                MethodInfo methodInfo = new MethodInfo(method.Name, className, framePath, new Stopwatch());

                int threadID = Thread.CurrentThread.ManagedThreadId;
                ThreadInfo threadInfo = _threads.GetOrAdd(threadID, new ThreadInfo());
                threadInfo.RunningMethods.Push(methodInfo);
                methodInfo.Stopwatch.Start();
            }
        }

        // Stop method tracing
        public void StopTrace()
        {
            int threadID = Thread.CurrentThread.ManagedThreadId;
            MethodInfo methodInfo;
            if (!_threads[threadID].RunningMethods.TryPop(out methodInfo))
                return;
            methodInfo.Stopwatch.Stop();
            _threads[threadID].Methods.Add(new MethodTrace(methodInfo.Name, methodInfo.ClassName, methodInfo.Stopwatch.Elapsed));
        }

        // Get trace result by threads
        public TraceResult GetTraceResult()
        {
            Dictionary<int, ThreadTrace> threads = new Dictionary<int, ThreadTrace>();

            foreach (var thread in _threads)
            {
                threads.Add(thread.Key, new ThreadTrace(thread.Key, thread.Value.Methods));
            }

            return new TraceResult(threads);
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