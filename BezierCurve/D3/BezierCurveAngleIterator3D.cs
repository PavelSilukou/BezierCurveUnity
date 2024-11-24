using System;
using System.Collections.Generic;
using BezierCurve.Utils;
using UnityEngine;

namespace BezierCurve
{
    public class BezierCurveAngleIterator3D : BezierCurveIterator3D
    {
        private readonly AngleThreshold _angleThreshold;
        private PositionAngle _currentPositionAngle;
        private readonly List<Arc> _arcs;

        public BezierCurveAngleIterator3D(IBezierCurve3D curve, float angle, float angleThreshold, bool returnLast) : base(curve, returnLast)
        {
            if (angle <= 0.0f) throw new ArgumentException($"Angle must be positive. Current value: {angle}");
            
            _angleThreshold = new AngleThreshold(angle, angleThreshold);
            _currentPositionAngle = new PositionAngle(0.0f, 0.0f);
            _arcs = CalculateArcs(curve, angle);
        }
        
        protected override BezierCurvePoint3D CalculateNext()
        {
            var point = new BezierCurvePoint3D(Curve, _currentPositionAngle.Position);
            
            var nextPositionAngle = GetNextPositionAngle(_currentPositionAngle);
            _currentPositionAngle = new PositionAngle(RoundClamp01(nextPositionAngle.Position), nextPositionAngle.Angle);
            
            return point;
        }

        protected override bool IsLast()
        {
            return FloatUtils.EqualsApproximately(_currentPositionAngle.Position, 1.0f);
        }

        private static List<Arc> CalculateArcs(IBezierCurve3D curve, float angle)
        {
            var result = new List<Arc>();
            
            var totalAngle = 0.0f;
            var lastSign = 0.0f;
            var lastAngleSign = 0;
            
            var steps = curve.Precision * curve.ControlPoints.Count;
            var precisionStep = 1.0f / steps;
            for (var i = 1; i <= steps; i++)
            {
                var step = Mathf.Clamp01(precisionStep * i);
                
                var sign = Mathf.Sign(curve.GetCurvature(step));
                if (FloatUtils.EqualsApproximately(lastSign, 0.0f) || !FloatUtils.EqualsApproximately(lastSign, sign))
                {
                    lastSign = sign;
                    lastAngleSign += 1;
                }
                
                var stepAngle = Vector3.Angle(curve.GetFirstDerivative(step - precisionStep),
                    curve.GetFirstDerivative(step));

                var innerSteps = (int)(stepAngle / angle);
                innerSteps = innerSteps == 0 ? 1 : innerSteps;
                var innerPrecisionStep = precisionStep / innerSteps;
                for (var j = 1; j <= innerSteps; j++)
                {
                    var innerStep = Mathf.Clamp01(precisionStep * (i - 1) + innerPrecisionStep * j);
                    var innerStepAngle = Vector3.Angle(curve.GetFirstDerivative(innerStep - innerPrecisionStep),
                        curve.GetFirstDerivative(innerStep));
                    totalAngle += innerStepAngle;

                    var arc = new Arc(innerStep, totalAngle, lastAngleSign);
                    result.Add(arc);
                }
            }

            return result;
        }

        private PositionAngle GetNextPositionAngle(PositionAngle positionAngle)
        {
            var currentArc = _arcs.Find(x => x.Angle > positionAngle.Angle);
            var targetArc = _arcs.Find(x => x.Angle >= positionAngle.Angle + _angleThreshold.Value);

            if (targetArc == null) return new PositionAngle(1.0f, positionAngle.Angle);
            
            if (currentArc.AngleSign != targetArc.AngleSign)
            {
                targetArc = _arcs.FindLast(x => x.AngleSign == currentArc.AngleSign);
                return new PositionAngle(targetArc.T, targetArc.Angle);
            }
            
            return CalculateNextPositionAngle(positionAngle, targetArc.T);
        }
        
        private PositionAngle CalculateNextPositionAngle(PositionAngle positionAngle, float endT)
        {
            var angle = Vector3.Angle(Curve.GetFirstDerivative(positionAngle.Position), Curve.GetFirstDerivative(endT));
            if (_angleThreshold.IsInRange(angle)) return new PositionAngle(endT, positionAngle.Angle + angle);

            var startT = positionAngle.Position;
            var halfT = positionAngle.Position + _angleThreshold.Value / (angle / (endT - positionAngle.Position));
            while (true)
            {
                angle = Vector3.Angle(Curve.GetFirstDerivative(positionAngle.Position), Curve.GetFirstDerivative(halfT));
                if (_angleThreshold.IsInRange(angle))
                {
                    return new PositionAngle(halfT, positionAngle.Angle + angle);
                }
                if (_angleThreshold.IsLess(angle))
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

        private sealed class AngleThreshold
        {
            public float Value { get; }
            public float Threshold { get; }

            public AngleThreshold(float value, float threshold)
            {
                Value = value;
                Threshold = threshold;
            }

            public bool IsInRange(float value)
            {
                return value > Value - Threshold && value < Value + Threshold;
            }

            public bool IsLess(float value)
            {
                return value > Value + Threshold;
            }
        }

        private sealed class PositionAngle
        {
            public float Position { get; }
            public float Angle { get; }

            public PositionAngle(float position, float angle)
            {
                Position = position;
                Angle = angle;
            }
        }

        private sealed class Arc
        {
            public float T { get; }
            public float Angle { get; }
            public int AngleSign { get; }

            public Arc(float t, float angle, int angleSign)
            {
                T = t;
                Angle = angle;
                AngleSign = angleSign;
            }
        }
    }
}
