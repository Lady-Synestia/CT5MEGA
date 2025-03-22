using System.Numerics;
using System.Runtime.CompilerServices;

namespace CT5MEGA;

public sealed partial record Matrix4D(Vector4D F, Vector4D U, Vector4D R, Vector4D W)
{
    public Vector4D F { get; } = F;
    public Vector4D U { get; } = U;
    public Vector4D R { get; } = R;
    public Vector4D W { get; } = W;

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

public sealed partial record Matrix4D
{

    public Vector3D EulerAngles()
    {
        /*
         * Maths for calculation from: https://www.opengl-tutorial.org/assets/faq_quaternions/index.html#Q37
         */
        
        double angleX; double angleZ;
        double angleY = Math.Asin(F.z);
        double c = Math.Cos(angleY);
        angleY *= Maths.Radians;

        if (Math.Abs(angleY) > 0.005f)
        {
            double trX = R.z / c;
            double trY = -U.z / c;
            angleX = Math.Atan2(trY, trX) * (180 / Math.PI);

            trX = F.x / c;
            trY = -F.y / c;
            angleZ = Math.Atan2(trY, trX) * (180 / Math.PI);
        }
        else
        {
            angleX = 0;
            double trX = U.y;
            double trY = U.x;
            angleZ = Math.Atan2(trY, trX) * (180 / Math.PI);
        }
        
        if (angleX < 0) { angleX += 360; }
        if (angleY < 0) { angleY += 360; }
        if (angleZ < 0) { angleZ += 360; }

        return new Vector3D(angleX, angleY, angleZ);
    }   
    
    
    public static Matrix4D Scale(double scaleX, double scaleY, double scaleZ) => new(
        new Vector3D(scaleX, 0, 0), 
        new Vector3D(0, scaleY, 0), 
        new Vector3D(0, 0, scaleZ));

    public static Matrix4D Scale(Vector3D scaleVector) => new(
        new Vector3D(scaleVector.x, 0, 0),
        new Vector3D(0, scaleVector.y, 0),
        new Vector3D(0, 0, scaleVector.z)
        );

    public static Matrix4D Scale(double scale) => new(
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
        
        angles *= Maths.Radians;
        
        double A = Math.Cos(angles.x);
        double B = Math.Sin(angles.x);
        double C = Math.Cos(angles.y);
        double D = Math.Sin(angles.y);
        double E = Math.Cos(angles.z);
        double F = Math.Sin(angles.z);

        double AD = A * D;
        double BD = B * D;

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
        
        double xx = Q.x * Q.x;
        double xy = Q.x * Q.y;
        double xz = Q.x * Q.z;
        double xw = Q.x * Q.w;
        
        double yy = Q.y * Q.y;
        double yz = Q.y * Q.z;
        double yw = Q.y * Q.w;
        
        double zz = Q.z * Q.z;
        double zw = Q.z * Q.w;
        
        return new Matrix4D(
            new Vector4D(1-2*(yy + zz), 2*(xy - zw), 2*(xz + yw), 0),
            new Vector4D(2*(xy + zw), 1-2*(xx + zz), 2*(yz-xw), 0),
            new Vector4D(2*(xz-yw), 2*(yz+xw), 1-2*(xx + yy), 0),
            new Vector4D(0, 0, 0, 1)
            );
    }
    
    // rotation matrix from axis-angle
    public static Matrix4D Rotation(double theta, Vector3D axis)
    {
        /*
         * Maths for calculation from: https://www.opengl-tutorial.org/assets/faq_quaternions/index.html#Q38
         */
        
        axis = axis.Normalised;
        theta *= Maths.Radians;
        
        double cosTheta = Math.Cos(theta);
        double sinTheta = Math.Sin(theta);

        return new Matrix4D(
            new Vector4D(cosTheta + axis.x*axis.x*(1-cosTheta), -axis.z*sinTheta + axis.x*axis.y*(1-cosTheta), axis.y*sinTheta + axis.x*axis.z*(1-cosTheta), 0),
            new Vector4D(axis.z * sinTheta + axis.y*axis.x*(1-cosTheta), cosTheta + axis.y*axis.y*(1-cosTheta), -axis.x*sinTheta + axis.y*axis.z*(1-cosTheta), 0),
            new Vector4D(-axis.y*sinTheta + axis.z*axis.x*(1-cosTheta), axis.x*sinTheta + axis.y*axis.z*(1-cosTheta), cosTheta + axis.z*axis.z*(1-cosTheta), 0),
            new Vector4D(0, 0, 0, 1)
            );
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