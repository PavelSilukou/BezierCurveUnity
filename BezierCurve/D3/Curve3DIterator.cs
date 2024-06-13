using BezierCurve.Utils;
using UnityEngine;

namespace BezierCurve
{
    public class Curve3DIterator
    {
        private readonly ICurve3D _curve;
        private float _currentPosition;
        private Vector3 _currentPoint;

        public Curve3DIterator(ICurve3D curve)
        {
            _curve = curve;
            _currentPosition = 0.0f;
            _curve.Build();
            _currentPoint = curve.GetPoint(_currentPosition);
        }

        public Vector3 GetPoint(float distance)
        {
            var shift = distance / _curve.Length;
            var newPosition = _currentPosition + shift;
            _currentPosition = Mathf.Clamp01(newPosition);
            _currentPoint = _curve.GetPoint(newPosition);
            return _currentPoint;
        }
        
        public Vector3 GetFirstDerivative()
        {
            return _curve.GetFirstDerivative(_currentPosition).normalized + _currentPoint;
        }
        
        public Vector3 GetSecondDerivative()
        {
            return _curve.GetSecondDerivative(_currentPosition).normalized + _currentPoint;
        }
        
        public Vector3 GetThirdDerivative()
        {
            return _curve.GetThirdDerivative(_currentPosition).normalized + _currentPoint;
        }

        public float GetCurvature()
        {
            return _curve.GetCurvature(_currentPosition);
        }
        
        public float GetTorsion()
        {
            return _curve.GetTorsion(_currentPosition);
        }

        public bool IsEnd()
        {
            return FloatUtils.EqualsApproximately(_currentPosition, 1.0f);
        }
    }
}
