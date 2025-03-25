namespace CT5MEGA;

public class BoundingSphere(Vector3D center, double radius)
{
    public double Radius { get; } = radius;
    public Vector3D Center { get; } = center;
    
    public override string ToString() => $"r: {Radius}, c: {Center}";

    public static bool SphereIntersection(BoundingSphere sphere1, BoundingSphere sphere2)
    {
        double radii =  sphere1.Radius + sphere2.Radius;
        return Vector3D.DistanceSquared(sphere1.Center, sphere2.Center) <= radii * radii;
    }

    public bool Contains(BoundingSphere other) => Vector3D.Distance(Center, other.Center) + other.Radius <= Radius; 
}