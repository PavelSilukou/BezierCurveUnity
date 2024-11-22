using System.Collections.Generic;
using UnityEngine;

namespace BezierCurve
{
	public interface IBezierCurve2D
	{
		public List<Vector2> ControlPoints { get; }
		public List<float> ControlPointRatios { get; }
		public int Precision { get; }
		public float Length { get; }
		public Vector2 GetPoint(float t);
		public Vector2 GetFirstDerivative(float t); // Velocity
		public Vector2 GetSecondDerivative(float t); // Acceleration
		public Vector2 GetThirdDerivative(float t);
		public float GetCurvature(float t);
	}
}