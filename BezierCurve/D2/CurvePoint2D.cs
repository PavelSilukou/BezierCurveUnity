using BezierCurve.Utils;
using UnityEngine;

namespace BezierCurve
{
	public class CurvePoint2D
	{
		private readonly ICurve2D _curve;
		private readonly float _t;

		internal CurvePoint2D(ICurve2D curve, float t)
		{
			_curve = curve;
			_t = t;
		}

		public Vector2 GetPoint()
		{
			return _curve.GetPoint(_t);
		}
		
		public Vector2 GetVelocity()
		{
			return _curve.GetFirstDerivative(_t);
		}

		public float GetCurvature()
		{
			return _curve.GetCurvature(_t);
		}
	}
}