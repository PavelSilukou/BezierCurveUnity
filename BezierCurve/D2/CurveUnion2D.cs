using System.Collections.Generic;
using BezierCurve.Utils;
using UnityEngine;

namespace BezierCurve
{
	public sealed class CurveUnion2D : ICurve2D
	{
		private readonly List<ICurve2D> _curves;
		private readonly List<float> _curvesLength;

		public CurveUnion2D(List<ICurve2D> curves)
		{
			_curves = curves;
			_curvesLength = new List<float>{ 0 };
			Length = 0;
			foreach (var c in _curves)
			{
				Length += c.Length;
				_curvesLength.Add(Length);
			}
		}
		
		public float Length { get; }

		public CurveIterator2D GetIterator(float distance, bool returnLast)
		{
			return new CurveIterator2D(this, distance, returnLast);
		}

		public Vector2 GetPoint(float t)
		{
			var pointData = CalculatePointData(t);
			return _curves[pointData.CurveId].GetPoint(pointData.T);
		}

		public Vector2 GetFirstDerivative(float t)
		{
			var pointData = CalculatePointData(t);
			return _curves[pointData.CurveId].GetFirstDerivative(pointData.T);
		}

		public Vector2 GetSecondDerivative(float t)
		{
			var pointData = CalculatePointData(t);
			return _curves[pointData.CurveId].GetSecondDerivative(pointData.T);
		}

		public Vector2 GetThirdDerivative(float t)
		{
			var pointData = CalculatePointData(t);
			return _curves[pointData.CurveId].GetThirdDerivative(pointData.T);
		}

		private PointData CalculatePointData(float t)
		{
			t = Mathf.Clamp01(t);

			if (FloatUtils.EqualsApproximately(t, 0.0f))
			{
				return new PointData(0.0f, 0);
			}
			
			var ratio = Length * t;
			var curvesLengthId = _curvesLength.FindIndex(length => length >= ratio);
			var newT = 
				(ratio - _curvesLength[curvesLengthId - 1]) /
				(_curvesLength[curvesLengthId] - _curvesLength[curvesLengthId - 1]);
			return new PointData(newT, curvesLengthId - 1);
		}

		private sealed class PointData
		{
			public readonly float T;
			public readonly int CurveId;

			public PointData(float t, int curveId)
			{
				T = t;
				CurveId = curveId;
			}
		}
	}
}