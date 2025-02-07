namespace CT5MEGA;


public sealed partial class Quaternion
{
    public float w { get; }
    public float x { get; } 
    public float y { get; }
    public float z { get; }
    
    // returns vector part
    public Vector3D v => new (x, y, z);
    
    // inverse of the quaternion
    public Quaternion Inverse => new (w, (-x, -y, -z));
    

    // constructor from real and vector parts
    public Quaternion(float w, (float x, float y, float z)tuple)
    {
        this.w = w;
        x = tuple.x;
        y = tuple.y;
        z = tuple.z;
    }
    
    // constructor from angle and axis parts
    public Quaternion(float angle, Vector3D axis)
    {
        const float deg2Rad = MathF.PI / 180;
        float halfAngle = angle * deg2Rad * 0.5f;
        w = MathF.Cos(halfAngle);
        x = axis.x * MathF.Sin(halfAngle);
        y = axis.y * MathF.Sin(halfAngle);
        z = axis.z * MathF.Sin(halfAngle);
    }

    // constructor for 0 real part
    public Quaternion(Vector3D v)
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
    // Multiplies quaternions
    public static Quaternion operator *(Quaternion a, Quaternion b) => new (b.w * a.w - Vector3D.Dot(b.v, a.v), (b.w * a.v + a.w * b.v + Vector3D.Cross(a.v, b.v)).Tuple);
    
    // rotates a vector by a quaternion
    public static Vector3D Rotate(Quaternion q, Vector3D p) => (q * new Quaternion(p) * q.Inverse).v;
}







