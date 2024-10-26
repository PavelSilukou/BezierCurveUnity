using System.Collections.Generic;
using UnityEngine;

namespace BezierCurve
{
	public static class CurveFactory
	{
		public static BezierCurve2D CreateBezierCurve2D(List<Vector2> controlPoints, int precision = 20)
		{
			var curve = new BezierCurve2D(controlPoints, precision);
			curve.Build();
			return curve;
		}
		
		public static BezierCurve2D CreateRationalBezierCurve2D(List<Vector2> controlPoints, List<float> controlPointsRatios, int precision = 20)
		{
			var curve = new RationalBezierCurve2D(controlPoints, controlPointsRatios, precision);
			curve.Build();
			return curve;
		}
		
		public static BezierCurve3D CreateBezierCurve3D(List<Vector3> controlPoints, int precision = 20)
		{
			var curve = new BezierCurve3D(controlPoints, precision);
			curve.Build();
			return curve;
		}
		
		public static BezierCurve3D CreateRationalBezierCurve3D(List<Vector3> controlPoints, List<float> controlPointsRatios, int precision = 20)
		{
			var curve = new RationalBezierCurve3D(controlPoints, controlPointsRatios, precision);
			curve.Build();
			return curve;
		}
	}
}