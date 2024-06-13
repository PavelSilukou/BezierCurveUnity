﻿using UnityEngine;

namespace BezierCurve
{
	public interface ICurve2D
	{
		public void Build();
		public float Length { get; }
		public Curve2DIterator GetIterator()
		{
			return new Curve2DIterator(this);
		}
		public Vector2 GetPoint(float t);
		public Vector2 GetFirstDerivative(float t); // Velocity
		public Vector2 GetSecondDerivative(float t); // Acceleration
		public Vector2 GetThirdDerivative(float t);
	}
}