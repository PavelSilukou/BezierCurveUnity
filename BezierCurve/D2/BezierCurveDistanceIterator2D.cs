using System;
using BezierCurve.Utils;
using UnityEngine;

namespace BezierCurve
{
    public class BezierCurveDistanceIterator2D
    {
        private readonly IBezierCurve2D _bezierCurve;
        private readonly float _shift;
        private bool _returnLast;
        private float _currentPosition;

        public BezierCurveDistanceIterator2D(IBezierCurve2D bezierCurve, float distance, bool returnLast)
        {
            if (distance <= 0.0f) throw new ArgumentException($"Distance must be positive. Current value: {distance}");
            
            _bezierCurve = bezierCurve;
            _shift = distance / _bezierCurve.Length;
            _returnLast = returnLast;
            _currentPosition = 0.0f;
        }

        public BezierCurvePoint2D? GetNext()
        {
            if (IsLast())
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
            
            var point = new BezierCurvePoint2D(_bezierCurve, _currentPosition);
            
            var newPosition = _currentPosition + _shift;
            _currentPosition = Mathf.Clamp01(newPosition);
            
            return point;
        }

        public bool HasNext()
        {
            return _returnLast && !IsLast();
        }

        private bool IsLast()
        {
            return FloatUtils.EqualsApproximately(_currentPosition, 1.0f);
        }
    }
}
