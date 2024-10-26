using System.Collections.Generic;
using BezierCurve.Utils;
using UnityEngine;

namespace BezierCurve
{
	public sealed class CurveUnion3D : ICurve3D
	{
		private readonly List<ICurve3D> _curves;
		private readonly List<float> _curvesLength;

		public CurveUnion3D(List<ICurve3D> curves)
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

		public Curve3DIterator GetIterator(float distance, bool returnLast)
		{
			return new Curve3DIterator(this, distance, returnLast);
		}

		public Vector3 GetPoint(float t)
		{
			var pointData = CalculatePointData(t);
			return _curves[pointData.CurveId].GetPoint(pointData.T);
		}

		public Vector3 GetFirstDerivative(float t)
		{
			var pointData = CalculatePointData(t);
			return _curves[pointData.CurveId].GetFirstDerivative(pointData.T);
		}

		public Vector3 GetSecondDerivative(float t)
		{
			var pointData = CalculatePointData(t);
			return _curves[pointData.CurveId].GetSecondDerivative(pointData.T);
		}

		public Vector3 GetThirdDerivative(float t)
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