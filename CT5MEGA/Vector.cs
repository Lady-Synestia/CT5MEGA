namespace CT5MEGA;

public abstract partial class Vector<T>(int n)
{
    protected int n { get; } = n;
    protected float[] values = new float[n];

    public void Set(Vector<T> v)
    {
        values = v.values;
    }
    
}