using BezierCurve.Utils;
using UnityEngine;

namespace BezierCurve
{
    public class BezierCurveIterator
    {
        private readonly BezierCurve _curve;
        private float _currentPosition;
        private Vector3 _currentPoint;

        public BezierCurveIterator(BezierCurve curve)
        {
            _curve = curve;
            _currentPosition = 0.0f;
            _curve.Build();
            _currentPoint = curve.GetPoint(_currentPosition);
        }

        public Vector3 GetPoint(float distance)
        {
            var shift = distance / _curve.GetLength();
            var newPosition = _currentPosition + shift;
            _currentPosition = Mathf.Clamp01(newPosition);
            _currentPoint = _curve.GetPoint(newPosition);
            return _currentPoint;
        }
        
        public Vector3 GetVelocity()
        {
            return _curve.GetVelocity(_currentPosition).normalized + _currentPoint;
        }

        public float GetCurvature()
        {
            return _curve.GetCurvature(_currentPosition);
        }

        public bool IsEnd()
        {
            return FloatUtils.EqualsApproximately(_currentPosition, 1.0f);
        }
    }
}
