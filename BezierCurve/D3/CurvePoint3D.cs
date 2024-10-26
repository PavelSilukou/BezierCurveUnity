using BezierCurve.Utils;
using UnityEngine;

namespace BezierCurve
{
	public class CurvePoint3D
	{
		private readonly ICurve3D _curve;
		private readonly float _t;

		internal CurvePoint3D(ICurve3D curve, float t)
		{
			_curve = curve;
			_t = t;
		}

		public Vector3 GetPoint()
		{
			return _curve.GetPoint(_t);
		}
		
		public Vector3 GetVelocity()
		{
			return _curve.GetFirstDerivative(_t);
		}

		public float GetCurvature()
		{
			return _curve.GetCurvature(_t);
		}
	}
}