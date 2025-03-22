namespace CT5MEGA;

public static class Maths
{
    public const float Pi = 3.141592653f;
    
    public const float Tolerance = 0.00001f;

    public const float Radians = 180 / Pi;

    public static float Clamp(float value)
    {
        if (MathF.Abs(value - MathF.Round(value)) < Tolerance)
        {
            return MathF.Round(value);
        }
        return value;
    }
}