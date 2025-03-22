
using System.Numerics;
using System.Runtime.InteropServices.Marshalling;

namespace CT5MEGA;


public sealed partial class Quaternion
{
    public float w { get; set; }
    public float x { get; set; } 
    public float y { get; set; }
    public float z { get; set; }
    
    // returns vector part
    public Vector3D v => new (x, y, z);
    
    // inverse of the quaternion
    public Quaternion Inverse => new (w, (-x, -y, -z));

    public float Magnitude => MathF.Sqrt(w*w + x*x + y*y + z*z);
    
    public Quaternion Normalised => new (w/Magnitude, (x/Magnitude, y/Magnitude, z/Magnitude));
    
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
        axis = axis.Normalised;
        angle *= MathF.PI / 180;
        float halfAngle = angle * 0.5f;
        w = MathF.Cos(halfAngle);
        x = axis.x * MathF.Sin(halfAngle);
        y = axis.y * MathF.Sin(halfAngle);
        z = axis.z * MathF.Sin(halfAngle);
    }

    public Quaternion(Vector3D angles)
    {
        angles *= MathF.PI / 180;
        
        Quaternion qx = new(angles.x, Vector3D.X);
        Quaternion qy = new(angles.y, Vector3D.Y);
        Quaternion qz = new(angles.z, Vector3D.Z);

        Quaternion qt = qx * qy;
        Quaternion q = qt * qz;
        w = q.w;
        x = q.x;
        y = q.y;
        z = q.z;
    }

    // constructor from rotation matrix
    public Quaternion(Matrix4D mat)
    {
        /*
         * Maths for calculation from: https://www.opengl-tutorial.org/assets/faq_quaternions/index.html#Q55
         */
        
        float trace = 1 + mat.F.x + mat.U.y + mat.R.z;
        if (trace > 0.000001)
        {
            float s = MathF.Sqrt(trace) * 2;
            w = 0.25f * s;
            x = (mat.R.y - mat.U.z) / s;
            y = (mat.F.z - mat.R.x) / s;
            z = (mat.U.x - mat.F.y) / s;
        }
        else if (mat.F.x - mat.U.y > 0.000001 && mat.F.x - mat.R.z > 0.000001)
        {
            float s = MathF.Sqrt(1.0f + mat.F.x - mat.U.y - mat.R.z) * 2;
            w = (mat.R.y - mat.U.z) / s;
            x = 0.25f * s;
            y = (mat.F.z + mat.R.x) / s;
            z = (mat.U.x + mat.U.y) / s;
        }
        else if (mat.U.y - mat.R.z > 0.000001)
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
    public Quaternion(Vector3D v, float w=0)
    {
        this.w = w;
        x = v.x;
        y = v.y;
        z = v.z;
    }
    
    public override string ToString() => $"{MathF.Round(w, 4)} + {MathF.Round(x, 4)}i + {MathF.Round(y, 4)}j + {MathF.Round(z, 4)}k";
    //public override string ToString() => $"{w} + {x}i + {y}j + {z}k";
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







