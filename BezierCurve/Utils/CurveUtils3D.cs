using UnityEngine;

namespace BezierCurve.Utils
{
	public static class CurveUtils3D
	{
		public static CurvePoint3D GetPointByDistance(this ICurve3D curve, float distance)
		{
			var t = distance / curve.Length;
			t = Mathf.Clamp01(t);
			return new CurvePoint3D(curve, t);
		}
		
		public static float GetCurvature(this ICurve3D curve, float t)
		{
			var firstDerivative = curve.GetFirstDerivative(t);
			var secondDerivative = curve.GetSecondDerivative(t);

			var sign = Mathf.Sign(
				(secondDerivative.z * firstDerivative.y - secondDerivative.y * firstDerivative.z) +
				(secondDerivative.x * firstDerivative.z - secondDerivative.z * firstDerivative.x) +
				(secondDerivative.y * firstDerivative.x - secondDerivative.x * firstDerivative.y)
			);

			return sign * Mathf.Sqrt(
				       Mathf.Pow(secondDerivative.z * firstDerivative.y - secondDerivative.y * firstDerivative.z, 2)
				       + Mathf.Pow(secondDerivative.x * firstDerivative.z - secondDerivative.z * firstDerivative.x, 2)
				       + Mathf.Pow(secondDerivative.y * firstDerivative.x - secondDerivative.x * firstDerivative.y, 2)
			       )
			       / Mathf.Pow(firstDerivative.x * firstDerivative.x + 
			                   firstDerivative.y * firstDerivative.y + 
			                   firstDerivative.z * firstDerivative.z, 
				       3.0f / 2.0f);
		}

		public static float GetTorsion(this ICurve3D curve, float t)
		{
			var firstDerivative = curve.GetFirstDerivative(t);
			var secondDerivative = curve.GetSecondDerivative(t);
			var thirdDerivative = curve.GetThirdDerivative(t);
			return (thirdDerivative.x * (firstDerivative.y * secondDerivative.z - secondDerivative.y * firstDerivative.z) +
			     thirdDerivative.y * (secondDerivative.x * firstDerivative.z - firstDerivative.x * secondDerivative.z) +
			     thirdDerivative.z * (firstDerivative.x * secondDerivative.y - secondDerivative.x * firstDerivative.y)) /
			    (Mathf.Pow(firstDerivative.y * secondDerivative.z - secondDerivative.y * firstDerivative.z, 2) + 
			     Mathf.Pow(secondDerivative.x * firstDerivative.z - firstDerivative.x * secondDerivative.z, 2) +
			     Mathf.Pow(firstDerivative.x * secondDerivative.y - secondDerivative.x * firstDerivative.y, 2));
		}
	}
}