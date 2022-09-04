
using Lab1_Tracer_.Core;

public class C
{
    private ITracer _tracer;

    public C(ITracer tracer)
    {
        _tracer = tracer;
    }

    public void M0()
    {
        M1();
        M2();
    }

    private void M1()
    {
        _tracer.StartTrace();
        Thread.Sleep(100);
        _tracer.StopTrace();
    }

    private void M2()
    {
        _tracer.StartTrace();
        Thread.Sleep(200);
        _tracer.StopTrace();
    }

    public static void Main()
    {
        C c = new C(new Tracer());
        c.M0();
        
        TraceResult tr = c._tracer.GetTraceResult();

    }
}