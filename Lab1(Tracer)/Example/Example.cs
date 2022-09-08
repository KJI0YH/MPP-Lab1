using Abstractions;
using System.Reflection;
using Tracer.Core;

public class Foo
{
    private Bar _bar;
    private ITracer _tracer;
    private List<ITracerResultSerializer> _serializers = new List<ITracerResultSerializer>();
    private string _serializersPath = "Plugins/Serializers";

    internal Foo(ITracer tracer)
    {
        _tracer = tracer;
        _bar = new Bar(_tracer);

    }

    public void MyMethod()
    {
        _tracer.StartTrace();
        Thread.Sleep(100);
        _bar.InnerMethod();
        _tracer.StopTrace();
    }

    public static void Main()
    {
        Tracer.Core.Tracer tracer = new Tracer.Core.Tracer();
        Foo foo = new Foo(tracer);
        foo.MyMethod();

        TraceResult tr = tracer.GetTraceResult();

        List<ITracerResultSerializer> serializers = foo.RefreshSerializers();
        foreach (var serializer in serializers)
        {
            using var file = new FileStream($"result.{serializer.Format}", FileMode.Create);
            serializer.Serialize(tr, file);
        }
    }

    public List<ITracerResultSerializer> RefreshSerializers()
    {
        _serializers.Clear();

        DirectoryInfo pluginDirectory = new DirectoryInfo(_serializersPath);
        if (!pluginDirectory.Exists)
            pluginDirectory.Create();

        //берем из директории все файлы с расширением .dll      
        var pluginFiles = Directory.GetFiles(_serializersPath, "*.dll");
        foreach (var file in pluginFiles)
        {

            Assembly asm = Assembly.LoadFrom(file);

            var types = asm.GetTypes().
                            Where(t => t.GetInterfaces().
                            Where(i => i.FullName == typeof(ITracerResultSerializer).FullName).Any());


            foreach (var type in types)
            {
                ITracerResultSerializer? serializer = asm.CreateInstance(type.FullName) as ITracerResultSerializer;

                if (serializer != null)
                {
                    _serializers.Add(serializer);
                }

            }
        }
        return _serializers;
    }
}

public class Bar
{
    private ITracer _tracer;

    internal Bar(ITracer tracer)
    {
        _tracer = tracer;
    }

    public void InnerMethod()
    {
        _tracer.StartTrace();
        Thread.Sleep(200);
        MegaInner();
        _tracer.StopTrace();
    }

    public void MegaInner()
    {
        _tracer.StartTrace();
        Thread.Sleep(300);
        _tracer.StopTrace();
    }
}