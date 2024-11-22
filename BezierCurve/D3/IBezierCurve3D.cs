using System.Collections.Generic;
using UnityEngine;

namespace BezierCurve
{
	public interface IBezierCurve3D
	{
		public List<Vector3> ControlPoints { get; }
		public List<float> ControlPointRatios { get; }
		public int Precision { get; }
		public float Length { get; }
		public Vector3 GetPoint(float t);
		public Vector3 GetFirstDerivative(float t); // Velocity
		public Vector3 GetSecondDerivative(float t); // Acceleration
		public Vector3 GetThirdDerivative(float t);
		public float GetCurvature(float t);
		public float GetTorsion(float t);
	}
}