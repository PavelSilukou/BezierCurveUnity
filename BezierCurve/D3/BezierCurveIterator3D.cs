using UnityEngine;

namespace BezierCurve
{
    public abstract class BezierCurveIterator3D
    {
        protected readonly IBezierCurve3D Curve;
        private bool _returnLast;

        protected BezierCurveIterator3D(IBezierCurve3D curve, bool returnLast)
        {
            Curve = curve;
            _returnLast = returnLast;
        }

        public BezierCurvePoint3D? GetNext()
        {
            return CheckIsLast() ? null : CalculateNext();
        }

        public bool HasNext()
        {
            return _returnLast || !IsLast();
        }

        protected abstract bool IsLast();

        protected abstract BezierCurvePoint3D CalculateNext();

        protected static float RoundClamp01(float value)
        {
            value = Mathf.Clamp01(value);
            return value >= 0.999f ? 1.0f : value;
        }

        private bool CheckIsLast()
        {
            if (!IsLast()) return false;
            if (!_returnLast) return true;
            _returnLast = false;
            return false;
        }
    }
}
