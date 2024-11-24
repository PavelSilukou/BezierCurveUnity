using System;
using BezierCurve.Utils;

namespace BezierCurve
{
    public class BezierCurveDistanceIterator2D : BezierCurveIterator2D
    {
        private readonly float _shift;
        private float _currentPosition;

        public BezierCurveDistanceIterator2D(NormalizedBezierCurve2D curve, float distance, bool returnLast) : base(curve, returnLast)
        {
            if (distance <= 0.0f) throw new ArgumentException($"Distance must be positive. Current value: {distance}");
            
            _shift = distance / curve.Length;
            _currentPosition = 0.0f;
        }

        protected override BezierCurvePoint2D CalculateNext()
        {
            var point = new BezierCurvePoint2D(Curve, _currentPosition);
            
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
