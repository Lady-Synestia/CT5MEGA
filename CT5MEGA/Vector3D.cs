using System.Numerics;
using System.Reflection.Metadata;

namespace CT5MEGA;

public sealed partial record Vector3D(double x, double y , double z)
{
    public double x { get; } = x;
    public double y { get; } = y;
    public double z { get; } = z;

    // copy constructor
    //public Vector3D(Vector3D a) : this (a.x, a.y, a.z) { }
    
    // implicit conversion from 4D Vector
    public static implicit operator Vector3D(Vector4D v) => new(v.x, v.y, v.z);
    
    // x: pitch, y: yaw, z: roll
    public static Vector3D FromAngles(Vector3D angles) => new (
        Math.Cos(angles.y) * Math.Cos(angles.x), 
        Math.Sin(angles.x), 
        Math.Cos(angles.x) * Math.Sin(angles.y));
    public static Vector3D FromAngles2D(double roll) => new(
        Math.Cos(roll), 
        Math.Sin(roll),
        0);


    public override string ToString() => $"{Math.Round(x, 4)}, {Math.Round(y, 4)}, {Math.Round(z, 4)}";
    
    // return values as tuple
    public (double x, double y, double z) Tuple => (x, y, z);
    
}

public sealed partial record Vector3D
{ // vector operations
    
    // using pythagoras
    public double Magnitude => Math.Sqrt(x * x + y * y + z * z);
    public Vector3D Normalised => Normalise(this);
    public static Vector3D Normalise(Vector3D a) => a / a.Magnitude;
    
    // comparing two Vectors
    public static bool Equals(Vector3D a, Vector3D b) => Math.Abs(a.x - b.x) < Maths.Tolerance && Math.Abs(a.y - b.y) < Maths.Tolerance && Math.Abs(a.z - b.z) < Maths.Tolerance;
    //public static bool operator ==(Vector3D a, Vector3D b) => Equals(a, b);
    //public static bool operator !=(Vector3D a, Vector3D b) => !Equals(a, b);
    
    // Dot product of two vectors
    public static double Dot(Vector3D a, Vector3D b) => (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
    public double Dot(Vector3D b) => Dot(this, b); // concise syntax
    
    // Distance between two vector positions
    public static double Distance(Vector3D a, Vector3D b)
    {
        double dx = a.x - b.x;
        double dy = a.y - b.y;
        double dz = a.z - b.z;
        
        return Math.Sqrt(dx*dx + dy*dy + dz*dz);
    }
    
    // Angle between two vectors
    public static double Angle(Vector3D a, Vector3D b) => Math.Acos(Dot(a, b) / (a.Magnitude * b.Magnitude));
    
    // Cross product of two vectors
    public static Vector3D Cross(Vector3D a, Vector3D b) => new(
        a.y * b.z - a.z * b.y, 
        a.z*b.x - a.x*b.z, 
        a.x*b.y - a.y*b.x);
    
    // clamps the magnitude of a Vector, returns new vector
    public static Vector3D ClampMagnitude(Vector3D a, double maxlength) => a.Magnitude > maxlength ? Normalise(a) * maxlength : a;
    
    // calculates the midpoint of two vectors - same as Lerp(a, b, 0.5)
    public static Vector3D Midpoint(Vector3D a, Vector3D b) => a + (b - a) * 0.5f;
    
    // calculates point on the line between a and b
    public static Vector3D Lerp(Vector3D a, Vector3D b, double t) => a + (b - a) * t;
    
    // calculates the vector from a to b
    public static Vector3D Between(Vector3D a, Vector3D b) => b - a;
    public Vector3D From(Vector3D other) => Between(other, this); // concise syntax for second operand
    public Vector3D To(Vector3D other) => Between(this, other); // concise syntax for first operand

    // returns a vector with the largest values of each of the vectors
    public static Vector3D Max(Vector3D a, Vector3D b) => new(
        Math.Max(a.x, b.x), 
        Math.Max(a.y, b.y), 
        Math.Max(a.z, b.z));
    
    // returns a vector with the smallest values of each of the vectors
    public static Vector3D Min(Vector3D a, Vector3D b) => new(
        Math.Min(a.x, b.x), 
        Math.Min(a.y, b.y), 
        Math.Min(a.z, b.z));
    
    // projects a vector onto another vector
    public static Vector3D Project(Vector3D a, Vector3D b) => b * Dot(a, b);
}

public sealed partial record Vector3D
{ // component-wise operations
    
    // vector * vector is the Scalar Product of the vectors
    public static Vector3D operator *(Vector3D a, Vector3D b) => new (
        a.x * b.x, 
        a.y * b.y, 
        a.z * b.z);
    public static Vector3D operator *(Vector3D v, double s) => new (
        v.x * s, 
        v.y * s, 
        v.z * s);
    public static Vector3D operator *(double s, Vector3D v) => v * s;
    
    public static Vector3D operator /(Vector3D a, Vector3D b) => new (
        a.x / b.x, 
        a.y / b.y, 
        a.z / b.z);
    public static Vector3D operator /(Vector3D a, double s) => new (
        a.x / s, 
        a.y / s, 
        a.z / s);
    
    public static Vector3D operator +(Vector3D a, Vector3D b) => new (
        a.x + b.x, 
        a.y + b.y, 
        a.z + b.z);
    
    public static Vector3D operator -(Vector3D a, Vector3D b) => new (
        a.x - b.x, 
        a.y - b.y, 
        a.z - b.z);
}

public sealed partial record Vector3D
{ // default vector values
    
    public static Vector3D Zero => new(0, 0, 0);
        
    public static Vector3D One => new (1, 1, 1);

    public static Vector3D X => new (1, 0, 0);
        
    public static Vector3D Y => new (0, 1, 0);
        
    public static Vector3D Z => new (0, 0, 1);
}