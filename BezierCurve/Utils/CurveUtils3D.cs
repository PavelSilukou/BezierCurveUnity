using UnityEngine;

namespace BezierCurve.Utils
{
	public static class CurveUtils3D
	{
		public static BezierCurvePoint3D GetPointByDistance(this IBezierCurve3D curve, float distance)
		{
			var t = distance / curve.Length;
			t = Mathf.Clamp01(t);
			return new BezierCurvePoint3D(curve, t);
		}
	}
}