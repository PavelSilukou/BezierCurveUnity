using System.Collections.Generic;
using System.Linq;
using BezierCurve.Utils;
using UnityEngine;

namespace BezierCurve
{
	public class BezierCurve2D : RationalBezierCurve2D
	{
		internal BezierCurve2D(List<Vector2> controlPoints, int precision = 1) : base(controlPoints, CalculateControlPointRatios(controlPoints), precision)
		{
		}

		public override Vector2 GetPoint(float t)
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
		
		public override Vector2 GetFirstDerivative(float t)
		{
			t = Mathf.Clamp01(t);
			
			var n = ControlPoints.Count - 1;

			var result = Vector2.zero;
			for (var i = 0; i <= n - 1; i++)
			{
				result += MathUtils.GetBernsteinBasisPolynomials(n - 1, i, t) *
				          (ControlPoints[i + 1] - ControlPoints[i]);
			}

			return n * result;
		}
		
		public override Vector2 GetSecondDerivative(float t)
		{
			t = Mathf.Clamp01(t);
			
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

		public override Vector2 GetThirdDerivative(float t)
		{
			t = Mathf.Clamp01(t);
			
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
		
		private static List<float> CalculateControlPointRatios(List<Vector2> controlPoints)
		{
			return controlPoints.Select(_ => 1.0f).ToList();
		}
	}
}