using System.Collections.Generic;
using System.Linq;
using BezierCurve.Utils;
using UnityEngine;

namespace BezierCurve
{
	public class BezierCurve3D : RationalBezierCurve3D
	{
		internal BezierCurve3D(List<Vector3> controlPoints, int precision = 1) : base(controlPoints, CalculateControlPointRatios(controlPoints), precision)
		{
		}

		public override Vector3 GetPoint(float t)
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
        
		public override Vector3 GetFirstDerivative(float t)
		{
			t = Mathf.Clamp01(t);
			var n = ControlPoints.Count - 1;

			var result = Vector3.zero;
			for (var i = 0; i <= n - 1; i++)
			{
				result += MathUtils.GetBernsteinBasisPolynomials(n - 1, i, t) *
				          (ControlPoints[i + 1] - ControlPoints[i]);
			}

			return n * result;
		}

		public override Vector3 GetSecondDerivative(float t)
		{
			t = Mathf.Clamp01(t);
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

		public override Vector3 GetThirdDerivative(float t)
		{
			t = Mathf.Clamp01(t);
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
		
		private static List<float> CalculateControlPointRatios(List<Vector3> controlPoints)
		{
			return controlPoints.Select(_ => 1.0f).ToList();
		}
	}
}