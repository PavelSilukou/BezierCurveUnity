using System;
using BezierCurve.Utils;
using UnityEngine;

namespace BezierCurve
{
    public class Curve3DIterator
    {
        private readonly ICurve3D _curve;
        private readonly float _shift;
        private bool _returnLast;
        private float _currentPosition;

        public Curve3DIterator(ICurve3D curve, float distance, bool returnLast)
        {
            if (distance <= 0.0f) throw new ArgumentException($"Distance must be positive. Current value: {distance}");
            
            _curve = curve;
            _shift = distance / _curve.Length;
            _returnLast = returnLast;
            _currentPosition = 0.0f;
        }

        public CurvePoint3D? GetNextPoint()
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
            
            var point = new CurvePoint3D(_curve, _currentPosition);
            
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
