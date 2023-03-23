using System;

namespace BezierCurve.Utils
{
    internal static class FloatUtils
    {
        public static bool EqualsApproximately(float a, float b, float tolerance = 0.0001f)
        {
            return Math.Abs(a - b) < tolerance;
        }
    }
}