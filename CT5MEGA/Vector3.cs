namespace CT5MEGA;

public sealed partial class Vector3(float x, float y , float z)
{
    public float x { get; set; } = x;
    public float y { get; set; } = y;
    public float z { get; set; } = z;

    // copy constructor
    public Vector3(Vector3 a) : this (a.x, a.y, a.z) { }
    
    // set values
    public void Set(Vector3 a)
    {
        x = a.x;
        y = a.y;
        z = a.z;
    }
    
    public override string ToString() => $"({x}, {y}, {z})";
    
    // return values as tuple
    public (float x, float y, float z) Tuple => (x, y, z);
}

public sealed partial class Vector3
{ // vector operations
    
    // using pythagoras
    public float Magnitude => MathF.Sqrt(x * x + y * y + z * z);

    public Vector3 Normalised => Normalise(this);
    public static Vector3 Normalise(Vector3 a) => a / a.Magnitude;
    public void Normalise() => Set(Normalise(this)); // sets vector to its normalised value
    
    // comparing two Vectors
    public static bool Equals(Vector3 a, Vector3 b)
    { 
        const float tolerance = 0.00001f;
        return Math.Abs(a.x - b.x) < tolerance && Math.Abs(a.y - b.y) < tolerance && Math.Abs(a.z - b.z) < tolerance;
    }
    public bool Equals(Vector3 other) => Equals(this, other); // concise syntax
    
    // Dot product of two vectors
    public static float Dot(Vector3 a, Vector3 b) => (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
    public float Dot(Vector3 b) => Dot(this, b); // concise syntax
    
    // Distance between two vector positions
    public static float Distance(Vector3 a, Vector3 b)
    {
        float dx = a.x - b.x;
        float dy = a.y - b.y;
        float dz = a.z - b.z;
        
        return MathF.Sqrt(dx*dx + dy*dy + dz*dz);
    }
    public float Distance(Vector3 other) => Distance(this, other); // concise syntax
    
    // Angle between two vectors
    public static float Angle(Vector3 a, Vector3 b) => MathF.Acos(Dot(a, b) / (a.Magnitude * b.Magnitude));
    public float Angle(Vector3 other) => Angle(this, other); // concise syntax
    
    // Cross product of two vectors
    public static Vector3 Cross(Vector3 a, Vector3 b) => new(a.y * b.z - a.z * b.y, a.z*b.x - a.x*b.z, a.x*b.y - a.y*b.x);
    public void Cross(Vector3 other) => Set(Cross(this, other)); // Concise syntax, acts on this

    // Scales a Vector by another Vector component-wise (wraps * operator)
    public static Vector3 Scale(Vector3 a, Vector3 b) => a * b;
    public void Scale(Vector3 other) => Set(Scale(this, other)); // concise syntax, acts on this
    
    // clamps the magnitude of a Vector, returns new vector
    public static Vector3 ClampMagnitude(Vector3 a, float maxlength) => a.Magnitude > maxlength ? Normalise(a) * maxlength : a;
    public void ClampMagnitude(float maxlength) => Set(ClampMagnitude(this, maxlength)); // concise syntax, acts on vector
    
    // calculates the midpoint of two vectors - same as Lerp(a, b, 0.5)
    public static Vector3 Midpoint(Vector3 a, Vector3 b) => a + (b - a) * 0.5f;
    
    // calculates point on the line between a and b
    public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => a + (b - a) * t;
    
    // calculates the vector from a to b
    public static Vector3 Between(Vector3 a, Vector3 b) => b - a;
    public Vector3 From(Vector3 other) => Between(other, this); // concise syntax for second operand
    public Vector3 To(Vector3 other) => Between(this, other); // concise syntax for first operand

    // returns a vector with the largest values of each of the vectors
    public static Vector3 Max(Vector3 a, Vector3 b) => new(Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z));
    public void Maximise(Vector3 b) => Set(Max(this, b)); // concise syntax, acts on this
    
    // returns a vector with the largest values of each of the vectors
    public static Vector3 Min(Vector3 a, Vector3 b) => new(Math.Min(a.x, b.x), Math.Min(a.y, b.y), Math.Min(a.z, b.z));
    public void Minimise(Vector3 b) => Set(Min(this, b)); // concise syntax, acts on this
    
    // projects a vector onto another vector
    public static Vector3 Project(Vector3 a, Vector3 b) => b * Dot(a, b);
    public void Project(Vector3 b) => Set(Project(this, b)); // concise syntax, acts on this
}

public sealed partial class Vector3
{ // component-wise operations
    
    // vector * vector is the Scalar Product of the vectors
    public static Vector3 operator *(Vector3 a, Vector3 b) => new (a.x * b.x, a.y * b.y, a.z * b.z);
    public static Vector3 operator *(Vector3 v, float s) => new (v.x * s, v.y * s, v.z * s);
    public static Vector3 operator *(float s, Vector3 v) => v * s;
    
    public static Vector3 operator /(Vector3 a, Vector3 b) => new (a.x / b.x, a.y / b.y, a.z / b.z);
    public static Vector3 operator /(Vector3 a, float s) => new (a.x / s, a.y / s, a.z / s);
    
    public static Vector3 operator +(Vector3 a, Vector3 b) => new (a.x + b.x, a.y + b.y, a.z + b.z);
    
    public static Vector3 operator -(Vector3 a, Vector3 b) => new (a.x - b.x, a.y - b.y, a.z - b.z);
}

public sealed partial class Vector3
{ // default vector values
    
    public static Vector3 Zero => new(0, 0, 0);
        
    public static Vector3 One => new (1, 1, 1);

    public static Vector3 X => new (1, 0, 0);
        
    public static Vector3 Y => new (0, 1, 0);
        
    public static Vector3 Z => new (0, 0, 1);
}