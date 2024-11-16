using System.Collections.Generic;
using BezierCurve.Utils;
using UnityEngine;

namespace BezierCurve
{
	public class BezierCurve2D : ICurve2D
	{
		protected readonly List<Vector2> ControlPoints;

		private readonly List<float> _arcsLength = new List<float> { 0 };
		private readonly int _precision;

		internal BezierCurve2D(List<Vector2> controlPoints, int precision = 20)
		{
			ControlPoints = controlPoints;
			_precision = precision;
		}
		
		public float Length { get; private set; }

		internal void Build()
		{
			var steps = _precision * ControlPoints.Count;
			var precision = 1.0f / (_precision * ControlPoints.Count);
			for (var i = 1; i <= steps; i++)
			{
				var step = Mathf.Clamp01(precision * i);
				var arcLength = Vector2.Distance(GetRawPoint(step - precision), GetRawPoint(step));
				Length += arcLength;
				_arcsLength.Add(Length);
			}
		}

		public CurveIterator2D GetIterator(float distance, bool returnLast)
		{
			return new CurveIterator2D(this, distance, returnLast);
		}

		public virtual Vector2 GetPoint(float t)
		{
			t = NormalizeT(t);
			var n = ControlPoints.Count - 1;

			var result = Vector2.zero;
			for (var i = 0; i <= n; i++)
			{
				result += MathUtils.GetBernsteinBasisPolynomials(n, i, t) * ControlPoints[i];
			}

			return result;
		}
		
		public virtual Vector2 GetFirstDerivative(float t)
		{
			t = NormalizeT(t);
			var n = ControlPoints.Count - 1;

			var result = Vector2.zero;
			for (var i = 0; i <= n - 1; i++)
			{
				result += MathUtils.GetBernsteinBasisPolynomials(n - 1, i, t) *
				          (ControlPoints[i + 1] - ControlPoints[i]);
			}

			return n * result;
		}

		public virtual Vector2 GetSecondDerivative(float t)
		{
			t = NormalizeT(t);
			var n = ControlPoints.Count - 1;

			var result = Vector2.zero;
			for (var i = 0; i <= n - 2; i++)
			{
				result += MathUtils.GetBernsteinBasisPolynomials(n - 2, i, t) *
				          (ControlPoints[i + 2] - ControlPoints[i + 1] -
				           (ControlPoints[i + 1] - ControlPoints[i])
				          );
			}

			return n * (n - 1) * result;
		}

		public virtual Vector2 GetThirdDerivative(float t)
		{
			t = NormalizeT(t);
			var n = ControlPoints.Count - 1;

			var result = Vector2.zero;
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

			return (index + (targetLength - beforeTargetLength) / (_arcsLength[index + 1] - beforeTargetLength)) /
			       (_arcsLength.Count - 1);
		}

		protected virtual Vector2 GetRawPoint(float t)
		{
			t = Mathf.Clamp01(t);

			var n = ControlPoints.Count - 1;

			var result = Vector2.zero;
			for (var i = 0; i <= n; i++)
			{
				result += MathUtils.GetBernsteinBasisPolynomials(n, i, t) * ControlPoints[i];
			}

			return result;
		}
	}
}