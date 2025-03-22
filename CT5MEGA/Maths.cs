namespace CT5MEGA;

public static class Maths
{
    public const double Tolerance = 0.00000001f;

    public const double Radians = Math.PI / 180;

    public static double Clamp(double value)
    {
        if (Math.Abs(value - Math.Round(value)) < Tolerance)
        {
            return Math.Round(value);
        }
        return value;
    }
    
    public static bool Equal(double x, double y) => Math.Abs(x - y) < Tolerance;
}