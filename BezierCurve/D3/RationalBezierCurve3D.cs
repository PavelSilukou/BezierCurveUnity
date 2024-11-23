using System;
using System.Collections.Generic;
using BezierCurve.Utils;
using UnityEngine;

namespace BezierCurve
{
	public class RationalBezierCurve3D : IBezierCurve3D
	{
		public List<Vector3> ControlPoints { get; }
		public List<float> ControlPointRatios { get; }
		public int Precision { get; }
		public float Length { get; private set; }

		internal RationalBezierCurve3D(List<Vector3> controlPoints, List<float> controlPointRatios, int precision = 1)
		{
			ControlPoints = controlPoints;
			ControlPointRatios = controlPointRatios;
			Precision = precision;
		}
		
		internal void Build()
		{
			var steps = Precision * ControlPoints.Count;
			var precision = 1.0f / steps;
			for (var i = 1; i <= steps; i++)
			{
				var step = Mathf.Clamp01(precision * i);
				var arcLength = Vector3.Distance(GetPoint(step - precision), GetPoint(step));
				Length += arcLength;
			}
		}
		
		public virtual Vector3 GetPoint(float t)
		{
			t = Mathf.Clamp01(t);

			var n = ControlPoints.Count - 1;
			var numerator = Vector3.zero;
			var denominator = 0.0f;
			for (var i = 0; i <= n; i++)
			{
				numerator += MathUtils.GetBernsteinBasisPolynomials(n, i, t) * ControlPoints[i] *
				             ControlPointRatios[i];
				denominator += MathUtils.GetBernsteinBasisPolynomials(n, i, t) * ControlPointRatios[i];
			}

			return numerator / denominator;
		}
		
		public virtual Vector3 GetFirstDerivative(float t)
		{
			t = Mathf.Clamp01(t);

			var n = ControlPoints.Count - 1;
			return n * GetWeight(0, n - 1, t) * GetWeight(1, n - 1, t) / Mathf.Pow(GetWeight(0, n, t), 2) *
			       (GetPointK(1, n - 1, t) - GetPointK(0, n - 1, t));
		}
		
		public virtual Vector3 GetSecondDerivative(float t)
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

		public virtual Vector3 GetThirdDerivative(float t)
		{
			throw new NotImplementedException();
		}

		public float GetCurvature(float t)
		{
			var firstDerivative = GetFirstDerivative(t);
			var secondDerivative = GetSecondDerivative(t);

			var sign = Mathf.Sign(
				(secondDerivative.z * firstDerivative.y - secondDerivative.y * firstDerivative.z) +
				(secondDerivative.x * firstDerivative.z - secondDerivative.z * firstDerivative.x) +
				(secondDerivative.y * firstDerivative.x - secondDerivative.x * firstDerivative.y)
			);

			return sign * Mathf.Sqrt(
				       Mathf.Pow(secondDerivative.z * firstDerivative.y - secondDerivative.y * firstDerivative.z, 2)
				       + Mathf.Pow(secondDerivative.x * firstDerivative.z - secondDerivative.z * firstDerivative.x, 2)
				       + Mathf.Pow(secondDerivative.y * firstDerivative.x - secondDerivative.x * firstDerivative.y, 2)
			       )
			       / Mathf.Pow(firstDerivative.x * firstDerivative.x + 
			                   firstDerivative.y * firstDerivative.y + 
			                   firstDerivative.z * firstDerivative.z, 
				       3.0f / 2.0f);
		}

		public float GetTorsion(float t)
		{
			var firstDerivative = GetFirstDerivative(t);
			var secondDerivative = GetSecondDerivative(t);
			var thirdDerivative = GetThirdDerivative(t);
			return (thirdDerivative.x * (firstDerivative.y * secondDerivative.z - secondDerivative.y * firstDerivative.z) +
			        thirdDerivative.y * (secondDerivative.x * firstDerivative.z - firstDerivative.x * secondDerivative.z) +
			        thirdDerivative.z * (firstDerivative.x * secondDerivative.y - secondDerivative.x * firstDerivative.y)) /
			       (Mathf.Pow(firstDerivative.y * secondDerivative.z - secondDerivative.y * firstDerivative.z, 2) + 
			        Mathf.Pow(secondDerivative.x * firstDerivative.z - firstDerivative.x * secondDerivative.z, 2) +
			        Mathf.Pow(firstDerivative.x * secondDerivative.y - secondDerivative.x * firstDerivative.y, 2));
		}

		private Vector3 GetPointK(int i, int k, float t)
		{
			var numerator = Vector3.zero;
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