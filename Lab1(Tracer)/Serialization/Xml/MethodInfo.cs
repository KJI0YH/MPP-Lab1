using System.Xml.Serialization;
using Tracer.Core;

namespace Xml
{
    public class MethodInfo
    {
        [XmlAttribute("name")]
        public string Name;

        [XmlAttribute("class")]
        public string ClassName;

        [XmlAttribute("time")]
        public string Time;

        [XmlElement("method")]
        public List<MethodInfo> Methods;

        public MethodInfo(string name, string className, TimeSpan time, List<MethodInfo> methods)
        {
            Name = name;
            ClassName = className;
            Time = string.Format("{0:f0}ms", time.TotalMilliseconds);
            Methods = methods;
        }

        public MethodInfo()
        {
            Name = String.Empty;
            ClassName = String.Empty;
            Time = String.Empty;
            Methods = new List<MethodInfo>();
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
