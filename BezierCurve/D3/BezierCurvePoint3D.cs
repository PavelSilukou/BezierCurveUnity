using UnityEngine;

namespace BezierCurve
{
	public class BezierCurvePoint3D
	{
		private readonly IBezierCurve3D _curve;
		private readonly float _t;

		internal BezierCurvePoint3D(IBezierCurve3D bezierCurve, float t)
		{
			_curve = bezierCurve;
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
		
		public float GetTorsion()
		{
			return _curve.GetTorsion(_t);
		}
	}
}