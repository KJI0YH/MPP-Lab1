namespace Lab1_Tracer_.Core
{
    public class ThreadTrace
    {
        public int ThreadID { get; private set; }
        public double Time { get; private set; }
        public List<MethodTrace> Methods = new List<MethodTrace>();

        public ThreadTrace(int threadID)
        {
            ThreadID = threadID;
        }

        // Add method to thread trace
        public void AddMethod(string name, string @class, List<string> framePath)
        {
            MethodTrace? method = FindMethod(framePath, name);

            if (method != null)
            {
                method.InnerMethods.Add(new MethodTrace(name, @class, method.Name));
            }
            else
            {
                Methods.Add(new MethodTrace(name, @class, framePath[0]));
            }
        }

        // Stop method from thread trace
        public void StopMethod(string name, string @class, List<string> framePath)
        {
            MethodTrace? method = FindMethod(framePath, name);
            if (method != null)
            {
                method.TimeMeasure();
            }

        }

        // Find method in thread trace
        private MethodTrace? FindMethod(List<string> framePath, string methodName)
        {
            List<MethodTrace> methods = Methods;

            List<string>.Enumerator enumerator = framePath.GetEnumerator();
            enumerator.MoveNext();

            MethodTrace? method = null;
            MethodTrace? parent = null;

            foreach (string path in framePath)
            {
                parent = methods.Find(item => item.ParentMethod == path);

                if (parent != null)
                {
                    methods = parent.InnerMethods;
                    method = parent;
                }
                if (method != null && method.Name == methodName)
                {
                    return method;
                }
            }
            return method;
        }
    }
}
