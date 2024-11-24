using System;
using BezierCurve.Utils;

namespace BezierCurve
{
	public class BezierCurveDistanceIterator3D : BezierCurveIterator3D
	{
		private readonly float _shift;
		private float _currentPosition;

		public BezierCurveDistanceIterator3D(NormalizedBezierCurve3D curve, float distance, bool returnLast) : base(curve, returnLast)
		{
			if (distance <= 0.0f) throw new ArgumentException($"Distance must be positive. Current value: {distance}");
            
			_shift = distance / curve.Length;
			_currentPosition = 0.0f;
		}

		protected override BezierCurvePoint3D CalculateNext()
		{
			var point = new BezierCurvePoint3D(Curve, _currentPosition);
            
			var newPosition = _currentPosition + _shift;
			_currentPosition = RoundClamp01(newPosition);
            
			return point;
		}

		protected override bool IsLast()
		{
			return FloatUtils.EqualsApproximately(_currentPosition, 1.0f);
		}
	}
}