using UnityEngine;

namespace BezierCurve.Utils
{
    internal static class Vector3Utils
    {
        public static Vector2 ToVector2XZ(Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }
    }
}
