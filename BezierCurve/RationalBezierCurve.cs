using UnityEngine;
using System.Collections.Generic;
using BezierCurve.Utils;

namespace BezierCurve
{
	public class RationalBezierCurve : BezierCurve
	{
		private readonly List<float> _controlPointsRatios;

		public RationalBezierCurve(List<Vector3> controlPoints, List<float> controlPointsRatios) : base(controlPoints)
		{
			_controlPointsRatios = controlPointsRatios;
		}
		
		public override Vector3 GetPoint(float t)
		{
			t = NormalizeT(t);

			var n = ControlPoints.Count - 1;
			var numerator = Vector3.zero;
			var denominator = 0.0f;
			for (var i = 0; i <= n; i++)
			{
				numerator += MathUtils.GetBernsteinBasisPolynomials(n, i, t) * ControlPoints[i] *
				             _controlPointsRatios[i];
				denominator += MathUtils.GetBernsteinBasisPolynomials(n, i, t) * _controlPointsRatios[i];
			}

			return numerator / denominator;
		}
		
		public override Vector3 GetVelocity(float t)
		{
			t = NormalizeT(t);

			var n = ControlPoints.Count - 1;
			return n * GetWeight(0, n - 1, t) * GetWeight(1, n - 1, t) / (Mathf.Pow(GetWeight(0, n, t), 2)) *
			       (GetPointK(1, n - 1, t) - GetPointK(0, n - 1, t));
		}
		
		public override float GetCurvature(float t)
		{
			var vel = Vector3Utils.ToVector2XZ(GetVelocity(t));
			var acc = Vector3Utils.ToVector2XZ(GetAcceleration(t));

			return -(acc.y * vel.x - acc.x * vel.y) / Mathf.Pow(vel.x * vel.x + vel.y * vel.y, 3.0f / 2.0f);
		}

		protected override Vector3 GetRawPoint(float t)
		{
			t = Mathf.Clamp01(t);

			var n = ControlPoints.Count - 1;

			var numerator = Vector3.zero;
			var denominator = 0.0f;
			for (var i = 0; i <= n; i++)
			{
				numerator += MathUtils.GetBernsteinBasisPolynomials(n, i, t) * ControlPoints[i] *
				             _controlPointsRatios[i];
				denominator += MathUtils.GetBernsteinBasisPolynomials(n, i, t) * _controlPointsRatios[i];
			}

			return numerator / denominator;
		}
		
		protected override Vector3 GetAcceleration(float t)
		{
			t = NormalizeT(t);
			
			var n = ControlPoints.Count - 1;
			var weight2N2 = GetWeight(2, n - 2, t);
			var weight0N = GetWeight(0, n, t);
			var weight0N2 = GetWeight(0, n - 2, t);
			var weight0N1 = GetWeight(0, n - 1, t);
			var weight1N1 = GetWeight(1, n - 1, t);

			var part1 = n * weight2N2 / (Mathf.Pow(weight0N, 3)) *
			            (2 * n * Mathf.Pow(weight0N1, 2) - (n - 1) * weight0N2 * weight0N -
			             2 * weight0N1 * weight0N) * (GetPointK(2, n - 2, t) - GetPointK(1, n - 2, t));
			var part2 = n * weight0N2 / (Mathf.Pow(weight0N, 3)) *
			            (2 * n * Mathf.Pow(weight1N1, 2) - (n - 1) * weight2N2 * weight0N -
			             2 * weight1N1 * weight0N) * (GetPointK(1, n - 2, t) - GetPointK(0, n - 2, t));

			return part1 - part2;
		}

		private Vector3 GetPointK(int i, int k, float t)
		{
			var numerator = Vector3.zero;
			var denominator = 0.0f;
			for (var j = 0; j <= k; j++)
			{
				numerator += MathUtils.GetBernsteinBasisPolynomials(k, j, t) * ControlPoints[i + j] *
				             _controlPointsRatios[i + j];
				denominator += MathUtils.GetBernsteinBasisPolynomials(k, j, t) * _controlPointsRatios[i + j];
			}

			return numerator / denominator;
		}

		private float GetWeight(int i, int k, float t)
		{
			var result = 0.0f;
			for (var j = 0; j <= k; j++)
			{
				result += MathUtils.GetBernsteinBasisPolynomials(k, j, t) * _controlPointsRatios[i + j];
			}

			return result;
		}
	}
}