
using System.Numerics;
using System.Runtime.InteropServices.Marshalling;

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
        float halfAngle = angle * (MathF.PI / 180) * 0.5f;
        w = MathF.Cos(halfAngle);
        axis *= MathF.Sin(halfAngle);
        x = axis.x;
        y = axis.y;
        z = axis.z;
    }

    // constructor from rotation matrix
    public Quaternion(Matrix4D mat)
    {
        float trace = 1 + mat.F.x + mat.U.y + mat.R.z;
        if (trace > 0.00000001)
        {
            float s = MathF.Sqrt(trace) * 2;
            w = 0.25f * s;
            x = (mat.R.y - mat.U.z) / s;
            y = (mat.F.z - mat.R.x) / s;
            z = (mat.U.x - mat.F.y) / s;
        }
        else if (mat.F.x > mat.U.y && mat.F.x < mat.R.z)
        {
            float s = MathF.Sqrt(1.0f + mat.F.x - mat.U.y - mat.R.z) * 2;
            w = (mat.R.y - mat.U.z) / s;
            x = 0.25f * s;
            y = (mat.F.z + mat.R.x) / s;
            z = (mat.U.x + mat.U.y) / s;
        }
        else if (mat.U.y > mat.R.z)
        {
            float s = MathF.Sqrt(1.0f + mat.U.y - mat.F.x - mat.R.z) * 2;
            w = (mat.U.z - mat.R.x) / s;
            x = (mat.U.x + mat.F.y) / s;
            y = 0.25f * s;
            z = (mat.R.y + mat.U.z) / s;
        }
        else
        {
            float s = MathF.Sqrt(1.0f + mat.R.z - mat.F.x - mat.U.y) * 2;
            w = (mat.U.x - mat.F.y) / s;
            x = (mat.F.z + mat.R.x) / s;
            y = (mat.R.y + mat.U.z) / s;
            z = 0.25f * s;
        }
    }

    public (float angle, Vector3D axis) GetAxisAngle()
    {
        float halfAngle = MathF.Acos(w);
        Vector3D a = new (x / MathF.Sin(halfAngle), y / MathF.Sin(halfAngle), z / MathF.Sin(halfAngle));
        return (halfAngle * (180 / MathF.PI) * 2, a);
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

    public static Quaternion Slerp(Quaternion a, Quaternion b, float t)
    {
        Quaternion d = b * a.Inverse;
        float wt = MathF.Cos(t * MathF.Acos(d.w));
        Vector3D vt = (d.v / MathF.Acos(d.w)) * MathF.Sin(t * MathF.Acos(d.w));
        Quaternion dt = new(wt, vt);
        return dt * a;
    }
    

    public static Quaternion Slerp2(Quaternion a, Quaternion b, float t)
    {
        Quaternion d = b * a.Inverse;
        (float angle, Vector3D axis) = d.GetAxisAngle();
        Quaternion dt = new(angle, axis);
        return dt * a;
    }
}







