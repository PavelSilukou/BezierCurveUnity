using UnityEngine;

namespace BezierCurve.Utils
{
	public static class CurveUtils2D
	{
		public static CurvePoint2D GetPointByDistance(this ICurve2D curve, float distance)
		{
			var t = distance / curve.Length;
			t = Mathf.Clamp01(t);
			return new CurvePoint2D(curve, t);
		}
		
		public static float GetCurvature(this ICurve2D curve, float t)
		{
			var firstDerivative = curve.GetFirstDerivative(t);
			var secondDerivative = curve.GetSecondDerivative(t);

			return (firstDerivative.y * secondDerivative.x - firstDerivative.x * secondDerivative.y) / 
			       Mathf.Pow(firstDerivative.x * firstDerivative.x + firstDerivative.y * firstDerivative.y, 
				       3.0f / 2.0f);
		}
	}
}