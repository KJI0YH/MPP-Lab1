namespace Tracer.Core
{
    public class MethodTrace
    {
        public string Name { get; private set; }
        public string Class { get; private set; }
        public TimeSpan Time { get; private set; }
        public string TimeStr
        {
            get
            {
                return String.Format("{0:f0}ms", Time.TotalMilliseconds);
            }
        }
        public List<MethodTrace> InnerMethods { get; private set; } = new List<MethodTrace>();

        public MethodTrace(string name, string @class, TimeSpan time)
        {
            Name = name;
            Class = @class;
            Time = time;
        }
    }
}
