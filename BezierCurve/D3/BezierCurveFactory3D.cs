using System.Collections.Generic;
using UnityEngine;

namespace BezierCurve
{
	public static class BezierCurveFactory3D
	{
		public static RationalBezierCurve3D CreateRationalBezierCurve(List<Vector3> controlPoints, List<float> controlPointRatios, int precision = 1)
		{
			var curve = new RationalBezierCurve3D(controlPoints, controlPointRatios, precision);
			curve.Build();
			return curve;
		}
		
		public static BezierCurve3D CreateBezierCurve(List<Vector3> controlPoints, int precision = 1)
		{
			var curve = new BezierCurve3D(controlPoints, precision);
			curve.Build();
			return curve;
		}
		
		public static NormalizedBezierCurve3D CreateNormalizedBezierCurve(IBezierCurve3D curve)
		{
			var normalizedCurve = new NormalizedBezierCurve3D(curve);
			normalizedCurve.Build();
			return normalizedCurve;
		}
		
		public static BezierCurveUnion3D CreateBezierCurveUnion(List<IBezierCurve3D> curves)
		{
			var curveUnion = new BezierCurveUnion3D(curves);
			curveUnion.Build();
			return curveUnion;
		}
	}
}