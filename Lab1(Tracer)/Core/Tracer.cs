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
            public Stopwatch Stopwatch { get; set; }
            public List<MethodInfo> InnerMethods { get; set; }

            public MethodInfo(string name, string className, Stopwatch stopwatch)
            {
                Name = name;
                ClassName = className;
                Stopwatch = stopwatch;
                InnerMethods = new List<MethodInfo>();
            }
        }

        private struct ThreadInfo
        {
            public ConcurrentStack<MethodInfo> RunningMethods { get; set; }
            public List<MethodInfo> RootMethods { get; set; }

            public ThreadInfo()
            {
                RunningMethods = new ConcurrentStack<MethodInfo>();
                RootMethods = new List<MethodInfo>();
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
                string className = method.DeclaringType == null ? string.Empty : method.DeclaringType.Name;
                MethodInfo methodInfo = new MethodInfo(method.Name, className, new Stopwatch());

                int threadID = Thread.CurrentThread.ManagedThreadId;
                ThreadInfo threadInfo = _threads.GetOrAdd(threadID, new ThreadInfo());

                if (threadInfo.RunningMethods.Count > 0)
                {
                    MethodInfo parentMethod = threadInfo.RunningMethods.First();
                    parentMethod.InnerMethods.Add(methodInfo);
                }
                else
                {
                    threadInfo.RootMethods.Add(methodInfo);
                }

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
        }

        // Get trace result by threads
        public TraceResult GetTraceResult()
        {
            List<ThreadTrace> threads = new List<ThreadTrace>();

            foreach (var thread in _threads)
            {
                List<MethodTrace> methods = new List<MethodTrace>();
                foreach (MethodInfo method in thread.Value.RootMethods)
                {
                    methods.Add(new MethodTrace(method.Name, method.ClassName, method.Stopwatch.Elapsed, GetInnerMethods(method)));
                }
                threads.Add(new ThreadTrace(thread.Key, methods));
            }

            return new TraceResult(threads);
        }

        private List<MethodTrace> GetInnerMethods(MethodInfo methodTrace)
        {
            List<MethodTrace> innerMethods = new List<MethodTrace>();
            foreach (MethodInfo method in methodTrace.InnerMethods)
            {
                innerMethods.Add(new MethodTrace(method.Name, method.ClassName, method.Stopwatch.Elapsed, GetInnerMethods(method)));
            }
            return innerMethods;
        }
    }
}