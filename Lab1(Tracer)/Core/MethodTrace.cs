namespace Tracer.Core
{
    public class MethodTrace
    {
        public string Name { get; private set; }
        public string Class { get; private set; }
        public TimeSpan Time { get; private set; }
        public IReadOnlyList<MethodTrace> InnerMethods { get; }

        public MethodTrace(string name, string @class, TimeSpan time, IReadOnlyList<MethodTrace> innerMethods)
        {
            Name = name;
            Class = @class;
            Time = time;
            InnerMethods = innerMethods;
        }
    }
}
