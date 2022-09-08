using System.Xml;
using System.Xml.Serialization;

namespace Xml
{
    public class ThreadInfo
    {
        [XmlAttribute("id")]
        public int ID;

        [XmlAttribute("time")]
        public string Time;

        [XmlElement("method")]
        public List<MethodInfo> Methods;

        public ThreadInfo(int id, double time, List<MethodInfo> methods)
        {
            ID = id;
            Time = string.Format("{0:f0}ms", time);
            Methods = methods;
        }

        public ThreadInfo()
        {
            ID = 0;
            Time = String.Empty;
            Methods = new List<MethodInfo>();
        }
    }
}
