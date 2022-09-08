namespace Yaml
{
    public class ThreadInfo
    {
        public int Id;
        public string Time;
        public List<MethodInfo> Methods;

        public ThreadInfo(int id, double time, List<MethodInfo> methods)
        {
            Id = id;
            Time = string.Format("{0:f0}ms", time);
            Methods = methods;
        }
    }
}
