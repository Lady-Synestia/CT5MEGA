using System.Runtime.CompilerServices;

namespace CT5MEGA;

public sealed partial record Vector4D(double x, double y, double z, double w)
{
    public double x { get; } = x;
    public double y { get; } = y;
    public double z { get; } = z;
    public double w { get; } = w;

    /*public Vector4D(Vector4D a) : this (
        a.x, 
        a.y, 
        a.z, 
        a.w) {}*/
    
    public Vector4D(Vector3D a, double w=1) : this (
        a.x, 
        a.y, 
        a.z, 
        w) {}
    
    public (double x, double y, double z, double w) Tuple => (x, y, z, w);
    
    // implicit conversion from 3D vector
    public static implicit operator Vector4D(Vector3D v) => new(
        v.x, 
        v.y, 
        v.z, 
        0);

    public override string ToString() => $"{Math.Round(x, 4)}, {Math.Round(y, 4)}, {Math.Round(z, 4)}, {Math.Round(w, 4)}";
}

public sealed partial record Vector4D
{
    public double Magnitude => Math.Sqrt(x * x + y * y + z * z + w * w);

    public Vector4D Normalised => Normalise(this);
    public static Vector4D Normalise(Vector4D a) => a / a.Magnitude;

    public static bool Equals(Vector4D a, Vector4D b) => Math.Abs(a.x - b.x) < Maths.Tolerance && Math.Abs(a.y - b.y) < Maths.Tolerance && Math.Abs(a.z - b.z) < Maths.Tolerance &&
               Math.Abs(a.w - b.w) < Maths.Tolerance;
    public static double Dot(Vector4D a, Vector4D b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
    
    public static double Angle(Vector4D a, Vector4D b) => Math.Acos(Dot(a, b) / (a.Magnitude * b.Magnitude));
    
}


public sealed partial record Vector4D
{
    public static Vector4D operator *(Vector4D a, Vector4D b) => new (
        a.x * b.x, 
        a.y * b.y, 
        a.z * b.z, 
        a.w * b.w);
    public static Vector4D operator *(Vector4D a, double s) => new (
        a.x * s, 
        a.y * s, 
        a.z * s, 
        a.w * s);
    public static Vector4D operator *(double s, Vector4D a) => a * s;
    
    public static Vector4D operator /(Vector4D a, Vector4D b) => new (
        a.x/b.x, 
        a.y/b.y, 
        a.z/b.z, 
        a.w/b.w);
    public static Vector4D operator /(Vector4D a, double s) => new (
        a.x/s, 
        a.y/s, 
        a.z/s, 
        a.w/s);
    
    public static Vector4D operator +(Vector4D a, Vector4D b) => new (
        a.x + b.x, 
        a.y + b.y, 
        a.z + b.z, 
        a.w + b.w);
    
    public static Vector4D operator -(Vector4D a, Vector4D b) => new (
        a.x - b.x, 
        a.y - b.y, 
        a.z - b.z, 
        a.w - b.w);
}


public sealed partial record Vector4D
{
    public static Vector4D Zero => new(0, 0, 0, 0);
    public static Vector4D One => new(1, 1, 1, 1);
    public static Vector4D X => new(1, 0, 0, 0);
    public static Vector4D Y => new(0, 1, 0, 0);
    public static Vector4D Z => new(0, 0, 1, 0);
    public static Vector4D W => new(0, 0, 0, 1);
    
}