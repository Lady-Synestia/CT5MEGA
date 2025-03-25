namespace CT5MEGA;

public class BoundingCapsule(Vector3D centre1, Vector3D centre2, double radius)
{
    public double Radius { get;} = radius;
    public Vector3D Centre1 { get; } = centre1;
    public Vector3D Centre2 { get; } = centre2;
    public double Height => Vector3D.Distance(Centre1, Centre2) + (2 * Radius);

    public override string ToString() => $"r: {Radius}, c1: {Centre1}, c2: {Centre2}";
    public bool SphereIntersect(BoundingSphere other)
    {
        double distanceSquared;
        
        if (Vector3D.Dot(Centre1, other.Center) < 0)
        {
            distanceSquared = Vector3D.DistanceSquared(Centre1, other.Center);
        }
        else if (Vector3D.Dot(Centre2, other.Center) < 0)
        {
            distanceSquared = Vector3D.DistanceSquared(Centre2, other.Center);
        }
        else
        {
            Vector3D AB = Centre1.To(Centre2);
            Vector3D AC = Centre1.To(other.Center);
            Vector3D BC = Centre2.To(other.Center);

            double ACdotAB = Vector3D.Dot(AB, AC);
            
            distanceSquared = AC.SqrMagnitude - ACdotAB * ACdotAB / AB.SqrMagnitude;
        }

        double radii = Radius + other.Radius;
        return distanceSquared <= radii * radii;
    }

    // only works if capsules are parallel
    public bool ParallelCapsuleIntersect(BoundingCapsule other)
    {
        Vector3D centre = Vector3D.Lerp(Centre1, Centre2, 0.5);
        Vector3D p = centre.To(other.Centre1);
        Vector3D q = centre.To(other.Centre2);

        if (p.SqrMagnitude < q.SqrMagnitude)
        {
            return SphereIntersect(new BoundingSphere(other.Centre1, other.Radius));
        }
        return SphereIntersect(new BoundingSphere(other.Centre2, other.Radius));
    }

    // only works if lines cross
    public bool CapsuleIntersect(BoundingCapsule other)
    {
        Vector3D d1 = Centre2 - Centre1;
        Vector3D d2 = other.Centre2 - other.Centre1;
        
        Vector3D a = other.Centre1 - Centre1;
        Vector3D b = Vector3D.Cross(d1, d2);
        
        double distance = Math.Abs(Vector3D.Dot(a, b)/b.Magnitude);
        
        return distance < Radius + other.Radius;
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}