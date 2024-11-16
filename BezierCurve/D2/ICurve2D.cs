using UnityEngine;

namespace BezierCurve
{
	public interface ICurve2D
	{
		public float Length { get; }
		public CurveIterator2D GetIterator(float distance, bool returnLast);
		public Vector2 GetPoint(float t);
		public Vector2 GetFirstDerivative(float t); // Velocity
		public Vector2 GetSecondDerivative(float t); // Acceleration
		public Vector2 GetThirdDerivative(float t);
	}
}