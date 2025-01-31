namespace CT5MEGA;

public sealed partial class Vector4(float x, float y, float z, float w)
{
    public float x {get; set;}
    public float y {get; set;}
    public float z {get; set;}
    public float w {get; set;}

    public Vector4(Vector4 a) : this (a.x, a.y, a.z, a.w) {}

    public Vector4(Vector3 a) : this (a.x, a.y, a.z, 0) {}
    
    public (float x, float y, float z, float w) Tuple => (x, y, z, w);
    
    public void Set(Vector4 a)
    {
        x = a.x;
        y = a.y;
        z = a.z;
        w = a.w;
    }
    
    public override string ToString() => $"({x}, {y}, {z}, {w})";
}

public sealed partial class Vector4
{
    public float Magnitude => MathF.Sqrt(x * x + y * y + z * z + w * w);

    public Vector4 Normalised => Normalise(this);
    public static Vector4 Normalise(Vector4 a) => a / a.Magnitude;
    public void Normalise() => Set(Normalise(this));

    public static bool Equals(Vector4 a, Vector4 b)
    {
        const float tolerance = 0.00001f;
        return Math.Abs(a.x - b.x) < tolerance && Math.Abs(a.y - b.y) < tolerance && Math.Abs(a.z - b.z) < tolerance &&
               Math.Abs(a.w - b.w) < tolerance;
    }
    
    public static float Dot(Vector4 a, Vector4 b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
    
    public static float Angle(Vector4 a, Vector4 b) => MathF.Acos(Dot(a, b) / (a.Magnitude * b.Magnitude));
    
}




public sealed partial class Vector4
{
    public static Vector4 operator *(Vector4 a, Vector4 b) => new (a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
    public static Vector4 operator *(Vector4 a, float s) => new (a.x * s, a.y * s, a.z * s, a.w * s);
    public static Vector4 operator *(float s, Vector4 a) => a * s;
    
    public static Vector4 operator /(Vector4 a, Vector4 b) => new (a.x/b.x, a.y/b.y, a.z/b.z, a.w/b.w);
    public static Vector4 operator /(Vector4 a, float s) => new (a.x/s, a.y/s, a.z/s, a.w/s);
    
    public static Vector4 operator +(Vector4 a, Vector4 b) => new (a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
    
    public static Vector4 operator -(Vector4 a, Vector4 b) => new (a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
}


public sealed partial class Vector4
{
    public static Vector4 Zero => new(0, 0, 0, 0);
    public static Vector4 One => new(1, 1, 1, 1);
    public static Vector4 X => new(1, 0, 0, 0);
    public static Vector4 Y => new(0, 1, 0, 0);
    public static Vector4 Z => new(0, 0, 1, 0);
    public static Vector4 W => new(0, 0, 0, 1);
}