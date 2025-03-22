namespace CT5MEGA;

public class AxisAlignedBoundingBox
{
    public Vector3D MinExtent { get;}
    public Vector3D MaxExtent { get;}

    public float Top => MaxExtent.y;
    public float Bottom => MinExtent.y;
    public float Right => MaxExtent.x;
    public float Left => MinExtent.x;
    public float Front => MaxExtent.z;
    public float Back => MinExtent.z;

    public AxisAlignedBoundingBox(Vector3D minExtent, Vector3D maxExtent)
    {
        MinExtent = minExtent;
        MaxExtent = maxExtent;
    }

    public AxisAlignedBoundingBox(Vector3D[] vertices)
    {
        Vector3D minExtent = vertices[0];
        Vector3D maxExtent = vertices[0];
        foreach (var vertex in vertices)
        {
            minExtent = Vector3D.Min(minExtent, vertex);
            maxExtent = Vector3D.Max(maxExtent, vertex);
        }
        MinExtent = minExtent;
        MaxExtent = maxExtent;
    }

    public override string ToString() => $"min: {MinExtent}\nmax: {MaxExtent}";

    public static bool Intersects(AxisAlignedBoundingBox a, AxisAlignedBoundingBox b) => !(a.Left > b.Right || b.Left > a.Right || a.Bottom > b.Top || b.Bottom > a.Top || a.Back > b.Front || b.Back > a.Front);
    
    public static bool IntersectingAxis(Vector3D axis, AxisAlignedBoundingBox box, Vector3D start, Vector3D end, ref float lowest, ref float highest)
    {
        float minimum = 0, maximum = 1;
        
        if (axis == Vector3D.X)
        {
            start.x = Maths.Clamp(start.x);
            end.x = Maths.Clamp(end.x);
            minimum = (box.Left - start.x) / (end.x - start.x);
            maximum = (box.Right - start.x) / (end.x - start.x);
        }
        else if (axis == Vector3D.Y)
        {
            start.y = Maths.Clamp(start.y);
            end.y = Maths.Clamp(end.y);
            minimum = (box.Bottom - start.y) / (end.y - start.y);
            maximum = (box.Top - start.y) / (end.y - start.y);
        }
        else if (axis == Vector3D.Z)
        {
            start.z = Maths.Clamp(start.z);
            end.z = Maths.Clamp(end.z);
            minimum = (box.Back - start.z) / (end.z - start.z);
            maximum = (box.Front - start.z) / (end.z - start.z);
        }
        
        if (float.IsNaN(minimum)) { minimum = 0; }
        if (float.IsNaN(maximum)) { maximum = 1; }
        
        // ensuring max > min
        if (minimum > maximum) { (minimum, maximum) = (maximum, minimum); }

        if (maximum < lowest || minimum > highest) { return false; }
        
        lowest = MathF.Max(minimum, lowest);
        highest = MathF.Min(maximum, highest);

        if (lowest > highest) { return false; }

        return true;
    }

    public static bool LineIntersection(AxisAlignedBoundingBox box, Vector3D start, Vector3D end,
        out Vector3D intersectionPoint)
    {
        float lowest = 0, highest = 1;
        intersectionPoint = Vector3D.Zero;

        if (!IntersectingAxis(Vector3D.X, box, start, end, ref lowest, ref highest))
            return false;
        if (!IntersectingAxis(Vector3D.Y, box, start, end, ref lowest, ref highest))
            return false;
        if (!IntersectingAxis(Vector3D.Z, box, start, end, ref lowest, ref highest))
            return false;
        
        intersectionPoint = Vector3D.Lerp(start, end, lowest);
        
        return true;
    }
}

