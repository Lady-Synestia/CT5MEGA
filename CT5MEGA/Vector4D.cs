using System.Runtime.CompilerServices;

namespace CT5MEGA;

public sealed partial class Vector4D(float x, float y, float z, float w)
{
    public float x { get; set; } = x;
    public float y { get; set; } = y;
    public float z { get; set; } = z;
    public float w { get; set; } = w;

    public Vector4D(Vector4D a) : this (a.x, a.y, a.z, a.w) {}
    public Vector4D(Vector3D a, float w=0) : this (a.x, a.y, a.z, w) {}
    
    public (float x, float y, float z, float w) Tuple => (x, y, z, w);

    // gets as a 3D Vector
    public Vector3D To3D => new(x, y, z); 
    
    public void Set(Vector4D a)
    {
        x = a.x;
        y = a.y;
        z = a.z;
        w = a.w;
    }
    
    public override string ToString() => $"({x}, {y}, {z}, {w})";
}

public sealed partial class Vector4D
{
    public float Magnitude => MathF.Sqrt(x * x + y * y + z * z + w * w);

    public Vector4D Normalised => Normalise(this);
    public static Vector4D Normalise(Vector4D a) => a / a.Magnitude;
    public void Normalise() => Set(Normalise(this));

    public static bool Equals(Vector4D a, Vector4D b)
    {
        const float tolerance = 0.00001f;
        return Math.Abs(a.x - b.x) < tolerance && Math.Abs(a.y - b.y) < tolerance && Math.Abs(a.z - b.z) < tolerance &&
               Math.Abs(a.w - b.w) < tolerance;
    }
    
    public static float Dot(Vector4D a, Vector4D b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
    
    public static float Angle(Vector4D a, Vector4D b) => MathF.Acos(Dot(a, b) / (a.Magnitude * b.Magnitude));
    
}


public sealed partial class Vector4D
{
    public static Vector4D operator *(Vector4D a, Vector4D b) => new (a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
    public static Vector4D operator *(Vector4D a, float s) => new (a.x * s, a.y * s, a.z * s, a.w * s);
    public static Vector4D operator *(float s, Vector4D a) => a * s;
    
    public static Vector4D operator /(Vector4D a, Vector4D b) => new (a.x/b.x, a.y/b.y, a.z/b.z, a.w/b.w);
    public static Vector4D operator /(Vector4D a, float s) => new (a.x/s, a.y/s, a.z/s, a.w/s);
    
    public static Vector4D operator +(Vector4D a, Vector4D b) => new (a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
    
    public static Vector4D operator -(Vector4D a, Vector4D b) => new (a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
}


public sealed partial class Vector4D
{
    public static Vector4D Zero => new(0, 0, 0, 0);
    public static Vector4D One => new(1, 1, 1, 1);
    public static Vector4D X => new(1, 0, 0, 0);
    public static Vector4D Y => new(0, 1, 0, 0);
    public static Vector4D Z => new(0, 0, 1, 0);
    public static Vector4D W => new(0, 0, 0, 1);
    
}