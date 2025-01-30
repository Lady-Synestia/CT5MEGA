namespace CT5MEGA;


public sealed partial class Quaternion
{
    public float w { get; }
    public float x { get; } 
    public float y { get; }
    public float z { get; }
    public Vector v => new (x, y, z);
    public Quaternion Inverse => new (w, (-x, -y, -z));

    public Quaternion(float w, (float x, float y, float z)tuple)
    {
        this.w = w;
        x = tuple.x;
        y = tuple.y;
        z = tuple.z;
    }
    
    public Quaternion(float angle, Vector axis)
    {
        const float deg2Rad = MathF.PI / 180;
        float halfAngle = angle * deg2Rad * 0.5f;
        w = MathF.Cos(halfAngle);
        x = axis.x * MathF.Sin(halfAngle);
        y = axis.y * MathF.Sin(halfAngle);
        z = axis.z * MathF.Sin(halfAngle);
    }

    public Quaternion(Vector v)
    {
        w = 0;
        x = v.x;
        y = v.y;
        z = v.z;
    }
    
    public override string ToString() => $"{w} + {x}i + {y}j + {z}k";
}

public sealed partial class Quaternion
{
    public static Quaternion operator *(Quaternion a, Quaternion b) => new (b.w * a.w - Vector.Dot(b.v, a.v), (b.w * a.v + a.w * b.v + Vector.Cross(a.v, b.v)).Tuple);
    
    public static Vector Rotate(Quaternion q, Vector p) => (q * new Quaternion(p) * q.Inverse).v;
}