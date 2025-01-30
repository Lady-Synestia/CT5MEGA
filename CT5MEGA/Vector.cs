namespace CT5MEGA;

public sealed partial class Vector(float x, float y , float z)
{
    public float x { get; set; } = x;
    public float y { get; set; } = y;
    public float z { get; set; } = z;

    // copy constructor
    public Vector(Vector a) : this (a.x, a.y, a.z) { }
    
    // set values
    public void Set(Vector a)
    {
        x = a.x;
        y = a.y;
        z = a.z;
    }
    
    public override string ToString() => $"({x}, {y}, {z})";
    
    // return values as tuple
    public (float x, float y, float z) Tuple => (x, y, z);
}

public sealed partial class Vector
{ // vector operations
    
    // using pythagoras
    public float Magnitude => MathF.Sqrt(x * x + y * y + z * z);

    public Vector Normalised => Normalise(this);
    public static Vector Normalise(Vector a) => a / a.Magnitude;
    public void Normalise() => Set(Normalise(this)); // sets vector to its normalised value
    
    // comparing two Vectors
    public static bool Equals(Vector a, Vector b)
    { 
        const float tolerance = 0.00001f;
        return Math.Abs(a.x - b.x) < tolerance && Math.Abs(a.y - b.y) < tolerance && Math.Abs(a.z - b.z) < tolerance;
    }
    public bool Equals(Vector other) => Equals(this, other); // concise syntax
    
    // Dot product of two vectors
    public static float Dot(Vector a, Vector b) => (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
    public float Dot(Vector b) => Dot(this, b); // concise syntax
    
    // Distance between two vector positions
    public static float Distance(Vector a, Vector b)
    {
        float dx = a.x - b.x;
        float dy = a.y - b.y;
        float dz = a.z - b.z;
        
        return MathF.Sqrt(dx*dx + dy*dy + dz*dz);
    }
    public float Distance(Vector other) => Distance(this, other); // concise syntax
    
    // Angle between two vectors
    public static float Angle(Vector a, Vector b) => MathF.Acos(Dot(a, b) / (a.Magnitude * b.Magnitude));
    public float Angle(Vector other) => Angle(this, other); // concise syntax
    
    // Cross product of two vectors
    public static Vector Cross(Vector a, Vector b) => new(a.y * b.z - a.z * b.y, a.z*b.x - a.x*b.z, a.x*b.y - a.y*b.x);
    public void Cross(Vector other) => Set(Cross(this, other)); // Concise syntax, acts on this

    // Scales a Vector by another Vector component-wise (wraps * operator)
    public static Vector Scale(Vector a, Vector b) => a * b;
    public void Scale(Vector other) => Set(Scale(this, other)); // concise syntax, acts on this
    
    // clamps the magnitude of a Vector, returns new vector
    public static Vector ClampMagnitude(Vector a, float maxlength) => a.Magnitude > maxlength ? Normalise(a) * maxlength : a;
    public void ClampMagnitude(float maxlength) => Set(ClampMagnitude(this, maxlength)); // concise syntax, acts on vector
    
    // calculates the midpoint of two vectors
    public static Vector Midpoint(Vector a, Vector b) => a + (b - a) * 0.5f;
    
    // calculates the vector from a to b
    public static Vector Between(Vector a, Vector b) => b - a;
    public Vector From(Vector other) => Between(other, this); // concise syntax for second operand
    public Vector To(Vector other) => Between(this, other); // concise syntax for first operand

    // returns a vector with the largest values of each of the vectors
    public static Vector Max(Vector a, Vector b) => new(Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z));
    public void Maximise(Vector b) => Set(Max(this, b)); // concise syntax, acts on this
    
    // returns a vector with the largest values of each of the vectors
    public static Vector Min(Vector a, Vector b) => new(Math.Min(a.x, b.x), Math.Min(a.y, b.y), Math.Min(a.z, b.z));
    public void Minimise(Vector b) => Set(Min(this, b)); // concise syntax, acts on this
    
    // projects a vector onto another vector
    public static Vector Project(Vector a, Vector b) => b * Dot(a, b);
    public void Project(Vector b) => Set(Project(this, b)); // concise syntax, acts on this
}

public sealed partial class Vector
{ // component-wise operations
    
    // vector * vector is the Scalar Product of the vectors
    public static Vector operator *(Vector a, Vector b) => new (a.x * b.x, a.y * b.y, a.z * b.z);
    public static Vector operator *(Vector v, float s) => new (v.x * s, v.y * s, v.z * s);
    public static Vector operator *(float s, Vector v) => new (v.x * s, v.y * s, v.z * s);
    
    public static Vector operator /(Vector a, Vector b) => new (a.x / b.x, a.y / b.y, a.z / b.z);
    public static Vector operator /(Vector a, float s) => new (a.x / s, a.y / s, a.z / s);
    public static Vector operator /(Vector v, int s) => new (v.x / s, v.y / s, v.z / s);
    
    public static Vector operator +(Vector a, Vector b) => new (a.x + b.x, a.y + b.y, a.z + b.z);
    
    public static Vector operator -(Vector a, Vector b) => new (a.x - b.x, a.y - b.y, a.z - b.z);
}

public sealed partial class Vector
{ // default vector values
    
    public static Vector Zero => new(0, 0, 0);
        
    public static Vector One => new (1, 1, 1);

    public static Vector X => new (1, 0, 0);
        
    public static Vector Y => new (0, 1, 0);
        
    public static Vector Z => new (0, 0, 1);
}