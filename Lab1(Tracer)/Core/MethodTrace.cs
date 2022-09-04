using System;
using System.Diagnostics;

namespace Lab1_Tracer_.Core
{
    public class MethodTrace
    {
        public string Name { get; private set; }
        public string Class { get; private set; }
        public TimeSpan Time { get; private set; }

        public List<MethodTrace> InnerMethods = new List<MethodTrace>();

        private Stopwatch _stopwatch = new Stopwatch();

        public MethodTrace(string name, string @class)
        {
            Name = name;
            Class = @class;
            _stopwatch.Start();
        }

        public void TimeMeasure()
        {
            _stopwatch.Stop();
            Time = _stopwatch.Elapsed;
        }
    }
}
