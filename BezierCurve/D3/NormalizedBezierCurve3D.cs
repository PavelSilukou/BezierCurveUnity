using System.Collections.Generic;
using BezierCurve.Utils;
using UnityEngine;

namespace BezierCurve
{
	public class NormalizedBezierCurve3D : IBezierCurve3D
	{
		public List<Vector3> ControlPoints { get; }
		public List<float> ControlPointRatios { get; }
		public int Precision { get; }
		public float Length { get; }
		
		private readonly IBezierCurve3D _curve;
		private readonly List<float> _arcsLength = new() { 0 };

		internal NormalizedBezierCurve3D(IBezierCurve3D curve)
		{
			_curve = curve;
			ControlPoints = _curve.ControlPoints;
			ControlPointRatios = _curve.ControlPointRatios;
			Precision = _curve.Precision;
			Length = _curve.Length;
		}
		
		internal void Build()
		{
			var steps = Precision * ControlPoints.Count;
			var precisionStep = 1.0f / steps;
			var length = 0.0f;
			for (var i = 1; i <= steps; i++)
			{
				var step = Mathf.Clamp01(precisionStep * i);
				var arcLength = Vector3.Distance(_curve.GetPoint(step - precisionStep), _curve.GetPoint(step));
				length += arcLength;
				_arcsLength.Add(length);
			}
		}
		
		public Vector3 GetPoint(float t)
		{
			t = NormalizeT(t);
			return _curve.GetPoint(t);
		}

		public Vector3 GetFirstDerivative(float t)
		{
			t = NormalizeT(t);
			return _curve.GetFirstDerivative(t);
		}

		public Vector3 GetSecondDerivative(float t)
		{
			t = NormalizeT(t);
			return _curve.GetSecondDerivative(t);
		}

		public Vector3 GetThirdDerivative(float t)
		{
			t = NormalizeT(t);
			return _curve.GetThirdDerivative(t);
		}

		public float GetCurvature(float t)
		{
			t = NormalizeT(t);
			return _curve.GetCurvature(t);
		}

		public float GetTorsion(float t)
		{
			t = NormalizeT(t);
			return _curve.GetTorsion(t);
		}

		private float NormalizeT(float t)
		{
			t = Mathf.Clamp01(t);
			if (FloatUtils.EqualsApproximately(t, 1.0f)) return t;
			
			var targetLength = Length * t;

			var index = _arcsLength.FindLastIndex(x => x <= targetLength);
			var beforeTargetLength = _arcsLength[index];
			
			return (index + (targetLength - beforeTargetLength) / (_arcsLength[index + 1] - beforeTargetLength)) /
			       (_arcsLength.Count - 1);
		}
	}
}