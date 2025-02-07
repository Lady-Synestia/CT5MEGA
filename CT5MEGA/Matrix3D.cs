namespace CT5MEGA;

public sealed class Matrix3D
{
    public Vector3D F { get; }
    public Vector3D U { get; }
    public Vector3D R { get; }

    // construction of basis matrix from forward vector
    public Matrix3D(Vector3D f)
    {
        F = f.Normalised;
        R = Vector3D.Cross(Vector3D.Y, F).Normalised;
        U = Vector3D.Cross(F, R).Normalised;
    }
    
    public Matrix3D(Vector3D f, Vector3D u, Vector3D r)
    {
        F = f;
        U = u;
        R = r;
    }
    
    public static Vector3D operator *(Matrix3D m, Vector3D v) => new(Vector3D.Dot(m.F, v), Vector3D.Dot(m.U, v),Vector3D.Dot(m.R, v));
    
    public static Matrix3D Identity => new(Vector3D.X, Vector3D.Y, Vector3D.Z);
}

