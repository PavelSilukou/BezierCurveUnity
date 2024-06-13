using BezierCurve.Utils;
using UnityEngine;

namespace BezierCurve
{
    public class Curve2DIterator
    {
        private readonly ICurve2D _curve;
        private float _currentPosition;
        private Vector2 _currentPoint;

        public Curve2DIterator(ICurve2D curve)
        {
            _curve = curve;
            _currentPosition = 0.0f;
            _curve.Build();
            _currentPoint = curve.GetPoint(_currentPosition);
        }

        public Vector2 GetPoint(float distance)
        {
            var shift = distance / _curve.Length;
            var newPosition = _currentPosition + shift;
            _currentPosition = Mathf.Clamp01(newPosition);
            _currentPoint = _curve.GetPoint(newPosition);
            return _currentPoint;
        }
        
        public Vector2 GetFirstDerivative()
        {
            return _curve.GetFirstDerivative(_currentPosition).normalized + _currentPoint;
        }
        
        public Vector2 GetSecondDerivative()
        {
            return _curve.GetSecondDerivative(_currentPosition).normalized + _currentPoint;
        }
        
        public Vector2 GetThirdDerivative()
        {
            return _curve.GetThirdDerivative(_currentPosition).normalized + _currentPoint;
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
