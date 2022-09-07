using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using Tracer.Serialization.Abstractions;

namespace Tracer.Core
{
    public class Tracer : ITracer
    {
        private readonly ConcurrentDictionary<int, ThreadInfo> _threads = new();
        private List<ITracerResultSerializer> _serializers = new();
        private string _serializerPath = "Plugins/Serializers";

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
            Dictionary<int, ThreadTrace> threads = new Dictionary<int, ThreadTrace>();

            foreach (var thread in _threads)
            {
                List<MethodTrace> methods = new List<MethodTrace>();
                foreach (MethodInfo method in thread.Value.RootMethods)
                {
                    methods.Add(new MethodTrace(method.Name, method.ClassName, method.Stopwatch.Elapsed, GetInnerMethods(method)));
                }
                threads.Add(thread.Key, new ThreadTrace(thread.Key, methods));
            }

            return new TraceResult(threads);
        }

        public List<ITracerResultSerializer> RefreshSerializers()
        {
            _serializers.Clear();

            DirectoryInfo pluginDirectory = new DirectoryInfo(_serializerPath);
            if (!pluginDirectory.Exists)
                pluginDirectory.Create();

            //берем из директории все файлы с расширением .dll      
            var pluginFiles = Directory.GetFiles(_serializerPath, "*.dll");
            foreach (var file in pluginFiles)
            {

                Assembly asm = Assembly.LoadFrom(file);

                var types = asm.GetTypes().
                                Where(t => t.GetInterfaces().
                                Where(i => i.FullName == typeof(ITracerResultSerializer).FullName).Any());


                foreach (var type in types)
                {
                    var plugin = asm.CreateInstance(type.FullName) as ITracerResultSerializer;
                    _serializers.Add(plugin);
                }
            }
            return _serializers;
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