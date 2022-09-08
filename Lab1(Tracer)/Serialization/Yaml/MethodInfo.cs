using Tracer.Core;

namespace Yaml
{
    public class MethodInfo
    {
        public string Name;

        public string Class;

        public string Time;

        public List<MethodInfo> Methods;

        public MethodInfo(string name, string className, TimeSpan time, List<MethodInfo> methods)
        {
            Name = name;
            Class = className;
            Time = string.Format("{0:f0}ms", time.TotalMilliseconds);
            Methods = methods;
        }

        public static List<MethodInfo> GetInnerMethods(MethodTrace method)
        {
            List<MethodInfo> innerMethods = new List<MethodInfo>();
            foreach (MethodTrace methodTrace in method.InnerMethods)
            {
                innerMethods.Add(new MethodInfo(methodTrace.Name, methodTrace.Class, methodTrace.Time, GetInnerMethods(methodTrace)));
            }
            return innerMethods;
        }
    }
}
