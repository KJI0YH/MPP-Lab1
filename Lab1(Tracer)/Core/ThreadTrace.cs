using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void AddMethod(string name, string @class)
        {
            Methods.Add(new MethodTrace(name, @class));
        }

        public void StopMethod(string name, string @class)
        {
            MethodTrace method = Methods.Find(item => item.Name == name && item.Class == @class);
            method.TimeMeasure();
        }
    }
}
