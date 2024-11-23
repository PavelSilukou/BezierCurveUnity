using System;
using System.Collections.Generic;
using BezierCurve.Utils;
using UnityEngine;

namespace BezierCurve
{
    public class BezierCurveAngleIterator2D
    {
        private readonly IBezierCurve2D _curve;
        private readonly float _angle;
        private readonly float _angleThreshold;
        private bool _returnLast;
        private float _currentPosition;
        private float _currentAngle;

        private readonly List<float> _arcsT = new() { 0.0f };
        private readonly List<float> _arcsAngle = new() { 0.0f };
        private readonly List<int> _arcsAngleSign = new() { 0 };

        public BezierCurveAngleIterator2D(IBezierCurve2D curve, float angle, float angleThreshold, bool returnLast)
        {
            if (angle <= 0.0f) throw new ArgumentException($"Angle must be positive. Current value: {angle}");
            
            _curve = curve;
            _angle = angle;
            _angleThreshold = angleThreshold;
            _returnLast = returnLast;
            _currentPosition = 0.0f;
            _currentAngle = 0.0f;
            
            var steps = _curve.Precision * _curve.ControlPoints.Count;
            var precisionStep = 1.0f / steps;

            var totalAngle = 0.0f;
            var lastSign = 0.0f;
            var lastAngleSign = 0;
            for (var i = 1; i <= steps; i++)
            {
                var step = Mathf.Clamp01(precisionStep * i);
                
                var sign = Mathf.Sign(_curve.GetCurvature(step));
                if (FloatUtils.EqualsApproximately(lastSign, 0.0f) || !FloatUtils.EqualsApproximately(lastSign, sign))
                {
                    lastSign = sign;
                    lastAngleSign += 1;
                }
                
                var stepAngle = Vector2.Angle(_curve.GetFirstDerivative(step - precisionStep),
                    _curve.GetFirstDerivative(step));

                var innerSteps = (int)(stepAngle / angle);
                innerSteps = innerSteps == 0 ? 1 : innerSteps;
                var innerPrecisionStep = precisionStep / innerSteps;
                for (var j = 1; j <= innerSteps; j++)
                {
                    var innerStep = Mathf.Clamp01(precisionStep * (i - 1) + innerPrecisionStep * j);
                    var innerStepAngle = Vector2.Angle(_curve.GetFirstDerivative(innerStep - innerPrecisionStep),
                        _curve.GetFirstDerivative(innerStep));
                    
                    totalAngle += innerStepAngle;
                    _arcsAngle.Add(totalAngle);
                    _arcsT.Add(innerStep);
                    _arcsAngleSign.Add(lastAngleSign);
                }
            }
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
            
            var point = new BezierCurvePoint2D(_curve, _currentPosition);
            
            var newPosition = GetNextPosition();
            _currentPosition = Mathf.Clamp01(newPosition);
            
            return point;
        }

        public bool HasNext()
        {
            return _returnLast || !IsLast();
        }

        private bool IsLast()
        {
            return FloatUtils.EqualsApproximately(_currentPosition, 1.0f);
        }

        private float GetNextPosition()
        {
            var arcIndex = _arcsAngle.FindIndex(x => x > _currentAngle);
            var targetArcIndex = _arcsAngle.FindIndex(x => x >= _currentAngle + _angle);

            if (targetArcIndex == -1) return 1.0f;
            
            if (_arcsAngleSign[arcIndex] == _arcsAngleSign[targetArcIndex])
            {
                var t = CalculateNewT(_arcsT[targetArcIndex]);
                var angle = Vector2.Angle(_curve.GetFirstDerivative(_currentPosition), _curve.GetFirstDerivative(t));
                _currentAngle += angle;
                return t;
            }
            else
            {
                targetArcIndex = _arcsAngleSign.FindLastIndex(x => x == _arcsAngleSign[arcIndex]);
                _currentAngle = _arcsAngle[targetArcIndex];
                return _arcsT[targetArcIndex];
            }
        }
        
        private float CalculateNewT(float endT)
        {
            var angle = Vector2.Angle(_curve.GetFirstDerivative(_currentPosition), _curve.GetFirstDerivative(endT));
            if (angle > _angle - _angleThreshold && angle < _angle + _angleThreshold) return endT;

            var startT = _currentPosition;
            var halfT = _currentPosition + _angle / (angle / (endT - _currentPosition));
            while (true)
            {
                angle = Vector2.Angle(_curve.GetFirstDerivative(_currentPosition), _curve.GetFirstDerivative(halfT));
                if (angle > _angle - _angleThreshold && angle < _angle + _angleThreshold)
                {
                    return halfT;
                }
                if (angle > _angle + _angleThreshold)
                {
                    endT = halfT;
                    halfT = (halfT - startT) / 2.0f + startT;
                }
                else
                {
                    startT = halfT;
                    halfT = (endT - halfT) / 2.0f + halfT;
                }
            }
        }
    }
}
