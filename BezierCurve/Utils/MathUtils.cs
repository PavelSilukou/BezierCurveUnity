using UnityEngine;

namespace BezierCurve.Utils
{
	internal static class MathUtils
	{
		private static int GetBinomialCoefficient(int n, int i)
		{
			return Factorial(n) / (Factorial(i) * Factorial(n - i));
		}

		private static int Factorial(int i)
		{
			if (i <= 1) return 1;
			return i * Factorial(i - 1);
		}

		public static float GetBernsteinBasisPolynomials(int n, int i, float t)
		{
			return GetBinomialCoefficient(n, i) * Mathf.Pow(t, i) * Mathf.Pow(1 - t, n - i);
		}
	}
}
