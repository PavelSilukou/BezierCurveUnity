using System;
using BezierCurve.Utils;
using UnityEngine;

namespace BezierCurve
{
    public class CurveIterator2D
    {
        private readonly ICurve2D _curve;
        private readonly float _shift;
        private bool _returnLast;
        private float _currentPosition;

        public CurveIterator2D(ICurve2D curve, float distance, bool returnLast)
        {
            if (distance <= 0.0f) throw new ArgumentException($"Distance must be positive. Current value: {distance}");
            
            _curve = curve;
            _shift = distance / _curve.Length;
            _returnLast = returnLast;
            _currentPosition = 0.0f;
        }

        public CurvePoint2D? GetNextPoint()
        {
            if (IsLastPoint())
            {
                if (_returnLast)
                {
                    _returnLast = false;
                }
                else
                {
                    return null;
                }
            }
            
            var point = new CurvePoint2D(_curve, _currentPosition);
            
            var newPosition = _currentPosition + _shift;
            _currentPosition = Mathf.Clamp01(newPosition);
            
            return point;
        }

        private bool IsLastPoint()
        {
            return FloatUtils.EqualsApproximately(_currentPosition, 1.0f);
        }
    }
}
