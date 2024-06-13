using System.Collections.Generic;
using BezierCurve.Utils;
using UnityEngine;

// https://pomax.github.io/bezierinfo/index.html
// https://www.mn.uio.no/math/english/people/aca/michaelf/papers/bez.pdf
namespace BezierCurve
{
	public class BezierCurve3D: ICurve3D
	{
		protected readonly List<Vector3> ControlPoints;

		private readonly List<float> _arcsLength = new List<float> { 0 };
		private readonly int _precision;

		public BezierCurve3D(List<Vector3> controlPoints, int precision = 20)
		{
			ControlPoints = controlPoints;
			_precision = precision;
		}

		public float Length { get; private set; }

		public void Build()
		{
			var precision = 1.0f / (_precision * ControlPoints.Count);

			for (var i = precision; i < 1.0f; i += precision)
			{
				var arcLength = Vector3.Distance(GetRawPoint(i - precision), GetRawPoint(i));
				Length += arcLength;
				_arcsLength.Add(Length);
			}
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
        
		public virtual Vector3 GetFirstDerivative(float t)
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

		public virtual Vector3 GetSecondDerivative(float t)
		{
			t = NormalizeT(t);
			var n = ControlPoints.Count - 1;

			var result = Vector3.zero;
			for (var i = 0; i <= n - 2; i++)
			{
				result += MathUtils.GetBernsteinBasisPolynomials(n - 2, i, t) *
				          (ControlPoints[i + 2] - ControlPoints[i + 1] -
				           (ControlPoints[i + 1] - ControlPoints[i])
				          );
			}

			return n * (n - 1) * result;
		}

		public virtual Vector3 GetThirdDerivative(float t)
		{
			t = NormalizeT(t);
			var n = ControlPoints.Count - 1;

			var result = Vector3.zero;
			for (var i = 0; i <= n - 3; i++)
			{
				result += MathUtils.GetBernsteinBasisPolynomials(n - 3, i, t) *
				          (ControlPoints[i + 3] - ControlPoints[i + 2] -
				           (ControlPoints[i + 2] - ControlPoints[i + 1] -
				            (ControlPoints[i + 1] - ControlPoints[i])
				           )
				          );
			}

			return n * (n - 1) * (n - 2) * result;
		}

		protected float NormalizeT(float t)
		{
			t = Mathf.Clamp01(t);
			var targetLength = Length * t;

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
	}
}