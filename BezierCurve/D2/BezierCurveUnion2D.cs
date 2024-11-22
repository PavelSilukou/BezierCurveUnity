using System.Collections.Generic;
using System.Linq;
using BezierCurve.Utils;
using UnityEngine;

namespace BezierCurve
{
	public sealed class BezierCurveUnion2D : IBezierCurve2D
	{
		public List<Vector2> ControlPoints { get; }
		public List<float> ControlPointRatios { get; }
		public int Precision { get; }
		public float Length { get; }

		private readonly List<IBezierCurve2D> _curves;
		private readonly List<float> _curvesLength = new() { 0 };

		internal BezierCurveUnion2D(List<IBezierCurve2D> curves)
		{
			_curves = curves;
			ControlPoints = curves.SelectMany(_ => ControlPoints).ToList();
			ControlPointRatios = curves.SelectMany(_ => ControlPointRatios).ToList();
			Precision = curves.Sum(curve => curve.Precision) / curves.Count;
			Length = curves.Sum(curve => curve.Length);
		}

		internal void Build()
		{
			var length = 0.0f;
			foreach (var c in _curves)
			{
				length += c.Length;
				_curvesLength.Add(length);
			}
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

		public float GetCurvature(float t)
		{
			var pointData = CalculatePointData(t);
			return _curves[pointData.CurveId].GetCurvature(pointData.T);
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