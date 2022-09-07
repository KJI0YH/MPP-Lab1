using Tracer.Core;
using Tracer.Serialization.Abstractions;

public class Foo
{
    private Bar _bar;
    private ITracer _tracer;

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

        List<ITracerResultSerializer> serializers = tracer.RefreshSerializers();
        foreach (var serializer in serializers)
        {
            using var file = new FileStream($"result.{serializer.Format}", FileMode.Create);
            serializer.Serialize(tr, file);
        }
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