using UnityEngine;

namespace BezierCurve
{
	public interface ICurve3D
	{
		public float Length { get; }
		public Curve3DIterator GetIterator(float distance, bool returnLast);
		public Vector3 GetPoint(float t);
		public Vector3 GetFirstDerivative(float t); // Velocity
		public Vector3 GetSecondDerivative(float t); // Acceleration
		public Vector3 GetThirdDerivative(float t);
	}
}