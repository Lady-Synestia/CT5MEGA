using System.Numerics;

namespace CT5MEGA;

public sealed partial class Matrix4X4(Vector4 f, Vector4 u, Vector4  r, Vector4 s)
{
    public Vector4 F { get; } = f;
    public Vector4 U { get; } = u;
    public Vector4 R { get; } = r;
    public Vector4 S { get; } = s;

    public Matrix4X4(Vector3 f) : this (new Matrix3X3(f)) {}
    public Matrix4X4(Matrix3X3 m) : this(m.F, m.U, m.R) { }
    public Matrix4X4(Vector3 f, Vector3 u, Vector3 r) : this (f, u, r, Vector3.Zero) { }
    public Matrix4X4(Vector3 f, Vector3 u, Vector3 r, Vector3 s) : this(new Vector4(f), new Vector4(u), new Vector4(r), new Vector4(s)) { }
}

public sealed partial class Matrix4X4
{
   // public static Matrix4X4 operator *(Matrix4X4 a, Matrix4X4 b) {}
}