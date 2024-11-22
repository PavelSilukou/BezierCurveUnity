using UnityEngine;

namespace BezierCurve.Utils
{
	public static class CurveUtils3D
	{
		public static BezierCurvePoint3D GetPointByDistance(this IBezierCurve3D bezierCurve, float distance)
		{
			var t = distance / bezierCurve.Length;
			t = Mathf.Clamp01(t);
			return new BezierCurvePoint3D(bezierCurve, t);
		}
	}
}