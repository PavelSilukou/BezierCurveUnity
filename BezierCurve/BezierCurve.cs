using UnityEngine;
using System.Collections.Generic;
using BezierCurve.Utils;

// https://pomax.github.io/bezierinfo/index.html
namespace BezierCurve
{
    public class BezierCurve
    {
        protected readonly List<Vector3> ControlPoints;
        
        private readonly List<float> _arcsLength = new List<float> { 0 };
        private readonly int _lengthPrecisionSteps;
        private float _length = 0.0f;

        public BezierCurve(List<Vector3> controlPoints, int lengthPrecisionSteps = 20)
        {
            ControlPoints = controlPoints;
            _lengthPrecisionSteps = lengthPrecisionSteps;
        }
        
        public BezierCurveIterator GetIterator(BezierCurve curve)
        {
            return new BezierCurveIterator(this);
        }
        
        public void Build()
        {
            var precision = 1.0f / (_lengthPrecisionSteps * ControlPoints.Count);

            for (var i = precision; i < 1.0f; i += precision)
            {
                var arcLength = Vector3.Distance(GetRawPoint(i - precision), GetRawPoint(i));
                _length += arcLength;
                _arcsLength.Add(_length);
            }
        }

        public float GetLength()
        {
            return _length;
        }

        public virtual Vector3 GetPoint(float t)
        {
            t = NormalizeT(t);
            var n = ControlPoints.Count - 1;

            var result = Vector3.zero;
            for (var i = 0; i <= n; i++)
            {
                result += MathUtils.GetBernsteinBasisPolynomials(n, i, t) * ControlPoints[i];
            }

            return result;
        }

        public virtual Vector3 GetVelocity(float t)
        {
            t = NormalizeT(t);
            var n = ControlPoints.Count - 1;

            var result = Vector3.zero;
            for (var i = 0; i <= n - 1; i++)
            {
                result += MathUtils.GetBernsteinBasisPolynomials(n - 1, i, t) *
                          (ControlPoints[i + 1] - ControlPoints[i]);
            }

            return n * result;
        }

        public virtual float GetCurvature(float t)
        {
            var vel = Vector3Utils.ToVector2XZ(GetVelocity(t));
            var acc = Vector3Utils.ToVector2XZ(GetAcceleration(t));

            return -(vel.x * acc.y - vel.y * acc.x) / Mathf.Pow(vel.x * vel.x + vel.y * vel.y, 3.0f / 2.0f);
        }

        protected float NormalizeT(float t)
        {
            t = Mathf.Clamp01(t);
            var targetLength = _length * t;
            
            var index = _arcsLength.FindLastIndex(x => x <= targetLength);
            var beforeTargetLength = _arcsLength[index];

            if (FloatUtils.EqualsApproximately(beforeTargetLength, targetLength))
            {
                return t;
            }
            
            return (index + (targetLength - beforeTargetLength) / (_arcsLength[index + 1] - beforeTargetLength)) /
                   (_arcsLength.Count - 1);
        }
        
        protected virtual Vector3 GetRawPoint(float t)
        {
            t = Mathf.Clamp01(t);

            var n = ControlPoints.Count - 1;

            var result = Vector3.zero;
            for (var i = 0; i <= n; i++)
            {
                result += MathUtils.GetBernsteinBasisPolynomials(n, i, t) * ControlPoints[i];
            }

            return result;
        }
        
        protected virtual Vector3 GetAcceleration(float t)
        {
            t = NormalizeT(t);
            var n = ControlPoints.Count - 1;

            var result = Vector3.zero;
            for (var i = 0; i <= n - 2; i++)
            {
                result += MathUtils.GetBernsteinBasisPolynomials(n - 2, i, t) * (ControlPoints[i + 2] -
                    ControlPoints[i + 1] - ControlPoints[i + 1] + ControlPoints[i]);
            }

            return n * (n - 1) * result;
        }
    }
}