namespace CT5MEGA;

public sealed partial class Matrix3X3
{
    public Vector3 F { get; }
    public Vector3 U { get; }
    public Vector3 R { get; }

    public Matrix3X3(Vector3 f)
    {
        F = f.Normalised;
        R = Vector3.Cross(Vector3.Y, F).Normalised;
        U = Vector3.Cross(F, R).Normalised;
    }

    public Matrix3X3(Vector3 f, Vector3 u, Vector3 r)
    {
        F = f;
        U = u;
        R = r;
    }
}

public sealed partial class Matrix3X3
{
    public static Vector3 operator *(Matrix3X3 m, Vector3 v) => new(Vector3.Dot(m.F, v), Vector3.Dot(m.U, v),Vector3.Dot(m.R, v));
}