using System.Numerics;

namespace CT5MEGA;

public sealed class Matrix4D(Vector4D f, Vector4D u, Vector4D  r, Vector4D w)
{
    public Vector4D F { get; } = f;
    public Vector4D U { get; } = u;
    public Vector4D R { get; } = r;
    public Vector4D W { get; } = w;

    // constructor from single forwards vector
    public Matrix4D(Vector3D f) : this (new Matrix3D(f)) {}
    
    // constructor from 3x3 matrix
    public Matrix4D(Matrix3D m) : this(m.F, m.U, m.R, Vector3D.Zero) { }
    
    // constructor from 3 3D vectors
    public Matrix4D(Vector3D f, Vector3D u, Vector3D r) : this (f, u, r, Vector3D.Zero) { }
    
    // constructor from 4 3D vectors
    public Matrix4D(Vector3D f, Vector3D u, Vector3D r, Vector3D s) : this(
        new Vector4D(f), 
        new Vector4D(u), 
        new Vector4D(r), 
        new Vector4D(s)) { }
    
    public static Matrix4D Identity => new (
        Vector4D.X, 
        Vector4D.Y, 
        Vector4D.Z, 
        Vector4D.W);
    
    public static Vector4D operator *(Matrix4D m, Vector4D v) => new(
        Vector4D.Dot(m.F, v), 
        Vector4D.Dot(m.U, v), 
        Vector4D.Dot(m.R, v), 
        Vector4D.Dot(m.W, v));
    public static Vector3D operator *(Matrix4D m, Vector3D v) => new(
        Vector3D.Dot(m.F.To3D, v), 
        Vector3D.Dot(m.U.To3D, v), 
        Vector3D.Dot(m.R.To3D, v));

    public static Matrix4D operator *(Matrix4D a, Matrix4D b) => new(
        a * b.F,
        a * b.U,
        a * b.R,
        a * b.W
        );
   
    public static Matrix4D Scale(float scaleX, float scaleY, float scaleZ) => new(
        new Vector3D(scaleX, 0, 0), 
        new Vector3D(0, scaleY, 0), 
        new Vector3D(0, 0, scaleZ));
    public static Matrix4D Scale(Vector3D scale) => Scale(scale.x, scale.y, scale.z);
    
    public static Matrix4D Translation(Vector3D worldPos) => new(
        Vector4D.X, 
        Vector4D.Y, 
        Vector4D.Z, 
        new Vector4D(worldPos, 1));

    public static Matrix4D Rotation(Vector3D angles)
    {
        Matrix4D roll = new(
            new Vector4D(MathF.Cos(angles.z), MathF.Sin(angles.z), 0, 0),
            new Vector4D(-MathF.Sin(angles.z), MathF.Cos(angles.z), 0, 0), 
            Vector4D.Z, 
            Vector4D.W);

        Matrix4D pitch = new(
            Vector4D.X,
            new Vector4D(0, MathF.Cos(angles.x), MathF.Sin(angles.x), 0),
            new Vector4D(0, -MathF.Sin(angles.x), MathF.Cos(angles.x), 0),
            Vector4D.W);

        Matrix4D yaw = new(
            new Vector4D(MathF.Cos(angles.y), 0, -MathF.Sin(angles.y), 0),
            Vector4D.Y,
            new Vector4D(MathF.Sin(angles.y), 0, MathF.Cos(angles.y), 0),
            Vector4D.W);
        
        return yaw * (pitch * roll);
    }
    
    public static Matrix4D TRS(Vector3D translationVector, Vector3D eulerAngles, Vector3D scaleVector) => Translation(translationVector) * (Rotation(eulerAngles) * Scale(scaleVector));
}