
using Lab1_Tracer_.Core;

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
        Tracer tracer = new Tracer();
        Foo foo = new Foo(tracer);
        foo.MyMethod();

        TraceResult tr = foo._tracer.GetTraceResult();
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