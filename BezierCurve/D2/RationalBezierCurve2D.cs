using System;
using System.Collections.Generic;
using BezierCurve.Utils;
using UnityEngine;

namespace BezierCurve
{
	public class RationalBezierCurve2D : IBezierCurve2D
	{
		public List<Vector2> ControlPoints { get; }
		public List<float> ControlPointRatios { get; }
		public int Precision { get; }
		public float Length { get; private set; }

		internal RationalBezierCurve2D(List<Vector2> controlPoints, List<float> controlPointRatios, int precision = 1)
		{
			ControlPoints = controlPoints;
			ControlPointRatios = controlPointRatios;
			Precision = precision;
		}
		
		internal void Build()
		{
			var steps = Precision * ControlPoints.Count;
			var precisionStep = 1.0f / (Precision * ControlPoints.Count);
			for (var i = 1; i <= steps; i++)
			{
				var step = Mathf.Clamp01(precisionStep * i);
				var arcLength = Vector2.Distance(GetPoint(step - precisionStep), GetPoint(step));
				Length += arcLength;
			}
		}
		
		public virtual Vector2 GetPoint(float t)
		{
			t = Mathf.Clamp01(t);
			
			var n = ControlPoints.Count - 1;
			var numerator = Vector2.zero;
			var denominator = 0.0f;
			for (var i = 0; i <= n; i++)
			{
				numerator += MathUtils.GetBernsteinBasisPolynomials(n, i, t) * ControlPoints[i] *
				             ControlPointRatios[i];
				denominator += MathUtils.GetBernsteinBasisPolynomials(n, i, t) * ControlPointRatios[i];
			}

			return numerator / denominator;
		}
		
		public virtual Vector2 GetFirstDerivative(float t)
		{
			t = Mathf.Clamp01(t);
			
			var n = ControlPoints.Count - 1;
			return n * GetWeight(0, n - 1, t) * GetWeight(1, n - 1, t) / Mathf.Pow(GetWeight(0, n, t), 2) *
			       (GetPointK(1, n - 1, t) - GetPointK(0, n - 1, t));
		}
		
		public virtual Vector2 GetSecondDerivative(float t)
		{
			t = Mathf.Clamp01(t);
			
			var n = ControlPoints.Count - 1;
			var weight2N2 = GetWeight(2, n - 2, t);
			var weight0N = GetWeight(0, n, t);
			var weight0N2 = GetWeight(0, n - 2, t);
			var weight0N1 = GetWeight(0, n - 1, t);
			var weight1N1 = GetWeight(1, n - 1, t);

			var part1 = n * weight2N2 / Mathf.Pow(weight0N, 3) *
			            (2 * n * Mathf.Pow(weight0N1, 2) - (n - 1) * weight0N2 * weight0N -
			             2 * weight0N1 * weight0N) * (GetPointK(2, n - 2, t) - GetPointK(1, n - 2, t));
			var part2 = n * weight0N2 / Mathf.Pow(weight0N, 3) *
			            (2 * n * Mathf.Pow(weight1N1, 2) - (n - 1) * weight2N2 * weight0N -
			             2 * weight1N1 * weight0N) * (GetPointK(1, n - 2, t) - GetPointK(0, n - 2, t));

			return part1 - part2;
		}

		public virtual Vector2 GetThirdDerivative(float t)
		{
			throw new NotImplementedException();
		}

		public virtual float GetCurvature(float t)
		{
			t = Mathf.Clamp01(t);
			
			var firstDerivative = GetFirstDerivative(t);
			var secondDerivative = GetSecondDerivative(t);

			return (firstDerivative.y * secondDerivative.x - firstDerivative.x * secondDerivative.y) / 
			       Mathf.Pow(firstDerivative.x * firstDerivative.x + firstDerivative.y * firstDerivative.y, 
				       3.0f / 2.0f);
		}

		private Vector2 GetPointK(int i, int k, float t)
		{
			var numerator = Vector2.zero;
			var denominator = 0.0f;
			for (var j = 0; j <= k; j++)
			{
				numerator += MathUtils.GetBernsteinBasisPolynomials(k, j, t) * ControlPoints[i + j] *
				             ControlPointRatios[i + j];
				denominator += MathUtils.GetBernsteinBasisPolynomials(k, j, t) * ControlPointRatios[i + j];
			}

			return numerator / denominator;
		}

		private float GetWeight(int i, int k, float t)
		{
			var result = 0.0f;
			for (var j = 0; j <= k; j++)
			{
				result += MathUtils.GetBernsteinBasisPolynomials(k, j, t) * ControlPointRatios[i + j];
			}

			return result;
		}
	}
}