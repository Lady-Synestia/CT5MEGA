using System.Numerics;
using System.Runtime.CompilerServices;

namespace CT5MEGA;

public sealed partial class Matrix4D(Vector4D f, Vector4D u, Vector4D r, Vector4D w)
{
    public Vector4D F { get; } = f;
    public Vector4D U { get; } = u;
    public Vector4D R { get; } = r;
    public Vector4D W { get; } = w;

    public override string ToString()
    {
        Matrix4D t = Transpose;
        return $"[ {t.F}\n  {t.U}\n  {t.R}\n  {t.W} ]";
    }

    // constructor from 3x3 matrix
    public static implicit operator Matrix4D(Matrix3D m) => new(m.F, m.U, m.R, Vector4D.W);

    // constructor from 3 3D vectors
    public Matrix4D(Vector3D f, Vector3D u, Vector3D r) : this(f, u, r, Vector4D.W) { }

    public Matrix4D(Matrix3D m, Vector4D w) : this(m.F, m.U, m.R, w) { }

    public static Matrix4D Identity => new(
        Vector4D.X,
        Vector4D.Y,
        Vector4D.Z,
        Vector4D.W);

    public Matrix4D Transpose => new(
        new Vector4D(F.x, U.x, R.x, W.x),
        new Vector4D(F.y, U.y, R.y, W.y),
        new Vector4D(F.z, U.z, R.z, W.z),
        new Vector4D(F.w, U.w, R.w, W.w)
    );

    // matrix multiplication is equal to taking the dot product between the transpose of the LHS and the RHS
    public static Vector4D operator *(Matrix4D m, Vector4D v) => m.Transpose.MultiplyTransposed(v);

    public static Vector4D operator *(Matrix4D m, Vector3D v) => m.Transpose.MultiplyTransposed(new Vector4D(v));
    
    public static Matrix4D operator *(Matrix4D a, Matrix4D b)
    {
        a = a.Transpose;
        return new Matrix4D(
            a.MultiplyTransposed(b.F),
            a.MultiplyTransposed(b.U),
            a.MultiplyTransposed(b.R),
            a.MultiplyTransposed(b.W)
        );
    }
    public Vector4D MultiplyTransposed(Vector4D v)=> new(
        Vector4D.Dot(F, v), 
        Vector4D.Dot(U, v), 
        Vector4D.Dot(R, v), 
        Vector4D.Dot(W, v));
}

public sealed partial class Matrix4D
{

    public Vector3D EulerAngles()
    {
        /*
         * Maths for calculation from: https://www.opengl-tutorial.org/assets/faq_quaternions/index.html#Q37
         */
        
        float angleX;
        float angleZ;
        float angleY = MathF.Asin(F.z);
        float c = MathF.Cos(angleY);
        angleY *= 180 / MathF.PI;

        if (MathF.Abs(angleY) > 0.005f)
        {
            float trX = R.z / c;
            float trY = -U.z / c;
            angleX = MathF.Atan2(trY, trX) * (180 / MathF.PI);

            trX = F.x / c;
            trY = -F.y / c;
            angleZ = MathF.Atan2(trY, trX) * (180 / MathF.PI);
        }
        else
        {
            angleX = 0;
            float trX = U.y;
            float trY = U.x;
            angleZ = MathF.Atan2(trY, trX) * (180 / MathF.PI);
        }
        
        if (angleX < 0) { angleX += 360; }
        if (angleY < 0) { angleY += 360; }
        if (angleZ < 0) { angleZ += 360; }

        return new Vector3D(angleX, angleY, angleZ);
    }   
    
    
    public static Matrix4D Scale(float scaleX, float scaleY, float scaleZ) => new(
        new Vector3D(scaleX, 0, 0), 
        new Vector3D(0, scaleY, 0), 
        new Vector3D(0, 0, scaleZ));

    public static Matrix4D Scale(Vector3D scaleVector) => new(
        new Vector3D(scaleVector.x, 0, 0),
        new Vector3D(0, scaleVector.y, 0),
        new Vector3D(0, 0, scaleVector.z)
        );

    public static Matrix4D Scale(float scale) => new(
        new Vector3D(scale, 0, 0),
        new Vector3D(0, scale, 0),
        new Vector3D(0, 0, scale)
        );
    
    public static Matrix4D Translation(Vector3D worldPos) => new(Matrix3D.Identity, new Vector4D(worldPos));

    // rotation matrix from euler angles
    public static Matrix4D Rotation(Vector3D angles)
    {
        /*
         * Maths for calculation from: https://www.opengl-tutorial.org/assets/faq_quaternions/index.html#Q36
         *
         * Simplified version of Matrix4D.RotationInefficient(Vector3D angles)
         */
        
        angles *= MathF.PI / 180;
        
        float A = MathF.Cos(angles.x);
        float B = MathF.Sin(angles.x);
        float C = MathF.Cos(angles.y);
        float D = MathF.Sin(angles.y);
        float E = MathF.Cos(angles.z);
        float F = MathF.Sin(angles.z);

        float AD = A * D;
        float BD = B * D;

        /*
         original code was incorrect as the example matrix was transposed
         
         return new Matrix4D(
            new Vector4D(C * E, BD * E + A * F, -AD * E + B * F,0),
            new Vector4D(-C * F, -BD * F + A * E, AD * F + B * E, 0),
            new Vector4D(D, -B * C, A * C, 0),
            new Vector4D(0, 0, 0, 1)
        );*/
        return new Matrix4D(
            new Vector4D(C * E,-C * F,D,0),
            new Vector4D(BD * E + A * F, -BD * F + A * E,-B * C,0),
            new Vector4D(-AD * E + B * F,AD * F + B * E,A * C,0),
            new Vector4D(0,0,0,1)
        );
    }

    // rotation matrix from quaternion
    public static Matrix4D Rotation(Quaternion Q)
    {
        /*
         * Maths for calculation from: https://www.opengl-tutorial.org/assets/faq_quaternions/index.html#Q54
         */
        
        //quaternion = quaternion.Normalised;
        
        float xx = Q.x * Q.x;
        float xy = Q.x * Q.y;
        float xz = Q.x * Q.z;
        float xw = Q.x * Q.w;
        
        float yy = Q.y * Q.y;
        float yz = Q.y * Q.z;
        float yw = Q.y * Q.w;
        
        float zz = Q.z * Q.z;
        float zw = Q.z * Q.w;
        
        return new Matrix4D(
            new Vector4D(1-2*(yy + zz), 2*(xy - zw), 2*(xz + yw), 0),
            new Vector4D(2*(xy + zw), 1-2*(xx + zz), 2*(yz-xw), 0),
            new Vector4D(2*(xz-yw), 2*(yz+xw), 1-2*(xx + yy), 0),
            new Vector4D(0, 0, 0, 1)
            );
    }
    
    // rotation matrix from axis-angle
    public static Matrix4D Rotation(float angle, Vector3D axis)
    {
        /*
         * Maths for calculation from: https://www.opengl-tutorial.org/assets/faq_quaternions/index.html#Q38
         */
        
        axis = axis.Normalised;
        angle *= MathF.PI / 180;
        
        float rcos = MathF.Cos(angle);
        float rsin = MathF.Sin(angle);

        return new Matrix4D(
            new Vector4D(rcos + axis.x*axis.x*(1-rcos), -axis.z*rsin + axis.x*axis.y*(1-rcos), axis.y*rsin + axis.x*axis.z*(1-rcos), 0),
            new Vector4D(axis.z * rsin + axis.y*axis.x*(1-rcos), rcos + axis.y*axis.y*(1-rcos), -axis.x*rsin + axis.y*axis.z*(1-rcos), 0),
            new Vector4D(-axis.y*rsin + axis.z*axis.x*(1-rcos), axis.x*rsin + axis.y*axis.z*(1-rcos), rcos + axis.z*axis.z*(1-rcos), 0),
            new Vector4D(0, 0, 0, 1)
            );
    }

    public static Matrix4D RotationInefficient(Vector3D angles)
    {
        // unsimplified version of Matrix4D.Rotation(Vector3D angles)
        
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

    // order of operations: Scale -> Rotate -> Translate
    public static Matrix4D TRS(Matrix4D translation, Matrix4D rotation, Matrix4D scale) => translation * (rotation * scale);
    
    
    public Matrix4D InverseTranslation => new(Matrix3D.Identity, new Vector4D(-W.x, -W.y, -W.z, 1));

    public Matrix4D InverseRotation => Transpose;

    public Matrix4D InverseScale => new Matrix3D(
        new Vector3D(1/F.x, 0, 0),
        new Vector3D(0, 1/U.y, 0),
        new Vector3D(0, 0, 1/R.z));
    
    // order of operations: Translate -> Rotate -> Scale
    public static Matrix4D SRT(Matrix4D scale, Matrix4D rotation, Matrix4D translation) => scale * (rotation * translation);
}