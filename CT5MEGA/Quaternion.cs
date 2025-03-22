
using System.Numerics;
using System.Runtime.InteropServices.Marshalling;

namespace CT5MEGA;


public sealed partial record Quaternion
{
    public double w { get; }
    public double x { get; } 
    public double y { get; }
    public double z { get; }
    
    // constructor from real and vector parts
    public Quaternion(double w, (double x, double y, double z)tuple)
    {
        this.w = w;
        x = tuple.x;
        y = tuple.y;
        z = tuple.z;
    }
        
    // returns vector part
    public Vector3D v => new (x, y, z);
    
    // inverse of the quaternion
    public Quaternion Inverse => new (w, (-x, -y, -z));

    public double Magnitude => Math.Sqrt(w*w + x*x + y*y + z*z);
    
    public Quaternion Normalised => new (w/Magnitude, (x/Magnitude, y/Magnitude, z/Magnitude));
    

    // constructor from angle and axis parts
    public Quaternion(double angle, Vector3D axis)
    {
        axis = axis.Normalised;
        angle *= Maths.Radians;
        double halfAngle = angle * 0.5f;
        w = Math.Cos(halfAngle);
        x = axis.x * Math.Sin(halfAngle);
        y = axis.y * Math.Sin(halfAngle);
        z = axis.z * Math.Sin(halfAngle);
    }

    public Quaternion(Vector3D angles)
    {
        angles *= Maths.Radians;
        
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
        
        double trace = 1 + mat.F.x + mat.U.y + mat.R.z;
        if (trace > Maths.Tolerance)
        {
            double s = Math.Sqrt(trace) * 2;
            w = 0.25f * s;
            x = (mat.R.y - mat.U.z) / s;
            y = (mat.F.z - mat.R.x) / s;
            z = (mat.U.x - mat.F.y) / s;
        }
        else if (mat.F.x - mat.U.y > Maths.Tolerance && mat.F.x - mat.R.z > Maths.Tolerance)
        {
            double s = Math.Sqrt(1.0 + mat.F.x - mat.U.y - mat.R.z) * 2;
            w = (mat.R.y - mat.U.z) / s;
            x = 0.25f * s;
            y = (mat.F.z + mat.R.x) / s;
            z = (mat.U.x + mat.U.y) / s;
        }
        else if (mat.U.y - mat.R.z > Maths.Tolerance)
        {
            double s = Math.Sqrt(1.0 + mat.U.y - mat.F.x - mat.R.z) * 2;
            w = (mat.U.z - mat.R.x) / s;
            x = (mat.U.x + mat.F.y) / s;
            y = 0.25f * s;
            z = (mat.R.y + mat.U.z) / s;
        }
        else
        {
            double s = Math.Sqrt(1.0 + mat.R.z - mat.F.x - mat.U.y) * 2;
            w = (mat.U.x - mat.F.y) / s;
            x = (mat.F.z + mat.R.x) / s;
            y = (mat.R.y + mat.U.z) / s;
            z = 0.25f * s;
        }
    }
    
    public (double angle, Vector3D axis) AxisAngle()
    {
        double halfAngle = Math.Acos(w);
        double sinH = Math.Sin(halfAngle);
        Vector3D axis = new (
            x / sinH, 
            y / sinH, 
            z / sinH);
        return (halfAngle / Maths.Radians * 2, axis);
    }

    // constructor for 0 real part
    public Quaternion(Vector3D v, double w=0)
    {
        this.w = w;
        x = v.x;
        y = v.y;
        z = v.z;
    }
    
    public override string ToString() => $"{Math.Round(w, 4)} + {Math.Round(x, 4)}i + {Math.Round(y, 4)}j + {Math.Round(z, 4)}k";
    //public override string ToString() => $"{w} + {x}i + {y}j + {z}k";
}

public sealed partial record Quaternion
{
    // Multiplies quaternions
    public static Quaternion operator *(Quaternion a, Quaternion b) => new (b.w * a.w - Vector3D.Dot(b.v, a.v), (b.w * a.v + a.w * b.v + Vector3D.Cross(a.v, b.v)).Tuple);
    
    // rotates a vector by a quaternion
    public static Vector3D Rotate(Quaternion q, Vector3D p) => (q * new Quaternion(p) * q.Inverse).v;

    public static Quaternion Slerp(Quaternion a, Quaternion b, double t)
    {
        Quaternion d = b * a.Inverse;
        double wt = Math.Cos(t * Math.Acos(d.w));
        Vector3D vt = (d.v / Math.Acos(d.w)) * Math.Sin(t * Math.Acos(d.w));
        Quaternion dt = new(wt, vt);
        return dt * a;
    }
    

    public static Quaternion Slerp2(Quaternion a, Quaternion b, double t)
    {
        Quaternion d = b * a.Inverse;
        (double angle, Vector3D axis) = d.AxisAngle();
        Quaternion dt = new(angle, axis);
        return dt * a;
    }
}







