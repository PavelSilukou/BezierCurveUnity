using UnityEngine;

namespace BezierCurve.Utils
{
	public static class CurveUtils2D
	{
		public static BezierCurvePoint2D GetPointByDistance(this IBezierCurve2D bezierCurve, float distance)
		{
			var t = distance / bezierCurve.Length;
			t = Mathf.Clamp01(t);
			return new BezierCurvePoint2D(bezierCurve, t);
		}
	}
}