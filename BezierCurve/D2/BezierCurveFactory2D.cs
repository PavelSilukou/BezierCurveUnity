using System.Collections.Generic;
using UnityEngine;

namespace BezierCurve
{
	public static class BezierCurveFactory2D
	{
		public static RationalBezierCurve2D CreateRationalBezierCurve(List<Vector2> controlPoints, List<float> controlPointRatios, int precision = 1)
		{
			var curve = new RationalBezierCurve2D(controlPoints, controlPointRatios, precision);
			curve.Build();
			return curve;
		}
		
		public static BezierCurve2D CreateBezierCurve(List<Vector2> controlPoints, int precision = 1)
		{
			var curve = new BezierCurve2D(controlPoints, precision);
			curve.Build();
			return curve;
		}
		
		public static NormalizedBezierCurve2D CreateNormalizedBezierCurve(IBezierCurve2D curve)
		{
			var normalizedCurve = new NormalizedBezierCurve2D(curve);
			normalizedCurve.Build();
			return normalizedCurve;
		}
		
		public static BezierCurveUnion2D CreateBezierCurveUnion(List<IBezierCurve2D> curves)
		{
			var curveUnion = new BezierCurveUnion2D(curves);
			curveUnion.Build();
			return curveUnion;
		}
	}
}