using UnityEngine;
using System.Drawing;
using System.Runtime.Versioning;

namespace BezierCurve.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        [SupportedOSPlatform("windows")]
        public void Test_BezierCurve4Points()
        {
            var points = new List<Vector3>
            {
                new (100.0f, 0.0f, 100.0f),
                new (100.0f, 0.0f, 400.0f),
                new (400.0f, 0.0f, 400.0f),
                new (400.0f, 0.0f, 100.0f)
            };
            var curve = new BezierCurve(points);
            curve.Build();

            Draw(curve, points);
            
            Assert.Pass();
        }
        
        [Test]
        [SupportedOSPlatform("windows")]
        public void Test_BezierCurve4PointsVelocity()
        {
            var points = new List<Vector3>
            {
                new (100.0f, 0.0f, 100.0f),
                new (100.0f, 0.0f, 400.0f),
                new (400.0f, 0.0f, 400.0f),
                new (400.0f, 0.0f, 100.0f)
            };
            var curve = new BezierCurve(points);
            curve.Build();

            DrawVelocity(curve, points);
            
            Assert.Pass();
        }
        
        [Test]
        [SupportedOSPlatform("windows")]
        public void Test_BezierCurve4PointsVelocityNormalized()
        {
            var points = new List<Vector3>
            {
                new (100.0f, 0.0f, 100.0f),
                new (100.0f, 0.0f, 400.0f),
                new (400.0f, 0.0f, 400.0f),
                new (400.0f, 0.0f, 100.0f)
            };
            var curve = new BezierCurve(points);
            curve.Build();

            DrawVelocityNormalized(curve, points);
            
            Assert.Pass();
        }

        [Test]
        [SupportedOSPlatform("windows")]
        public void Test_BezierCurve4PointsSignCurvature()
        {
            var points = new List<Vector3>
            {
                new (100.0f, 0.0f, 100.0f),
                new (100.0f, 0.0f, 250.0f),
                new (250.0f, 0.0f, 250.0f),
                new (250.0f, 0.0f, 400.0f)
            };
            var curve = new BezierCurve(points);
            curve.Build();

            DrawSignedCurvature(curve, points);
            
            Assert.Pass();
        }
        
        [Test]
        [SupportedOSPlatform("windows")]
        public void Test_BezierCurve4PointsSignCurvatureNormalized()
        {
            var points = new List<Vector3>
            {
                new (100.0f, 0.0f, 100.0f),
                new (100.0f, 0.0f, 250.0f),
                new (250.0f, 0.0f, 250.0f),
                new (250.0f, 0.0f, 400.0f)
            };
            var curve = new BezierCurve(points);
            curve.Build();

            DrawSignedCurvatureNormalized(curve, points);
            
            Assert.Pass();
        }
        
        [Test]
        [SupportedOSPlatform("windows")]
        public void Test_RationalBezierCurve4Points()
        {
            var points = new List<Vector3>
            {
                new (100.0f, 0.0f, 100.0f),
                new (100.0f, 0.0f, 400.0f),
                new (400.0f, 0.0f, 400.0f),
                new (400.0f, 0.0f, 100.0f)
            };
            var ratios = new List<float>
            {
                1.0f,
                2.0f,
                2.0f,
                1.0f
            };
            var curve = new RationalBezierCurve(points, ratios);
            curve.Build();

            Draw(curve, points);
            
            Assert.Pass();
        }
        
        [Test]
        [SupportedOSPlatform("windows")]
        public void Test_RationalBezierCurve4PointsVelocity()
        {
            var points = new List<Vector3>
            {
                new (100.0f, 0.0f, 100.0f),
                new (100.0f, 0.0f, 400.0f),
                new (400.0f, 0.0f, 400.0f),
                new (400.0f, 0.0f, 100.0f)
            };
            var ratios = new List<float>
            {
                1.0f,
                2.0f,
                2.0f,
                1.0f
            };
            var curve = new RationalBezierCurve(points, ratios);
            curve.Build();

            DrawVelocity(curve, points);
            
            Assert.Pass();
        }
        
        [Test]
        [SupportedOSPlatform("windows")]
        public void Test_RationalBezierCurve4PointsVelocityNormalized()
        {
            var points = new List<Vector3>
            {
                new (100.0f, 0.0f, 100.0f),
                new (100.0f, 0.0f, 400.0f),
                new (400.0f, 0.0f, 400.0f),
                new (400.0f, 0.0f, 100.0f)
            };
            var ratios = new List<float>
            {
                1.0f,
                2.0f,
                2.0f,
                1.0f
            };
            var curve = new RationalBezierCurve(points, ratios);
            curve.Build();

            DrawVelocityNormalized(curve, points);
            
            Assert.Pass();
        }

        [Test]
        [SupportedOSPlatform("windows")]
        public void Test_RationalBezierCurve4PointsSignCurvature()
        {
            var points = new List<Vector3>
            {
                new (100.0f, 0.0f, 100.0f),
                new (100.0f, 0.0f, 250.0f),
                new (250.0f, 0.0f, 250.0f),
                new (250.0f, 0.0f, 400.0f)
            };
            var ratios = new List<float>
            {
                1.0f,
                2.0f,
                2.0f,
                1.0f
            };
            var curve = new RationalBezierCurve(points, ratios);
            curve.Build();

            DrawSignedCurvature(curve, points);
            
            Assert.Pass();
        }
        
        [Test]
        [SupportedOSPlatform("windows")]
        public void Test_RationalBezierCurve4PointsSignCurvatureNormalized()
        {
            var points = new List<Vector3>
            {
                new (100.0f, 0.0f, 100.0f),
                new (100.0f, 0.0f, 250.0f),
                new (250.0f, 0.0f, 250.0f),
                new (250.0f, 0.0f, 400.0f)
            };
            var ratios = new List<float>
            {
                1.0f,
                2.0f,
                2.0f,
                1.0f
            };
            var curve = new RationalBezierCurve(points, ratios);
            curve.Build();

            DrawSignedCurvatureNormalized(curve, points);
            
            Assert.Pass();
        }

        [SupportedOSPlatform("windows")]
        private static void Draw(BezierCurve curve, List<Vector3> points)
        {
            var bitmap = new Bitmap(500, 500);
            
            DrawCurve(bitmap, curve);

            foreach (var point in points)
            {
                DrawPixel(bitmap, point);
                DrawPoint(bitmap, point);
            }

            bitmap.Save(TestContext.CurrentContext.Test.Name  + ".bmp");
        }
        
        [SupportedOSPlatform("windows")]
        private static void DrawVelocity(BezierCurve curve, List<Vector3> points)
        {
            var bitmap = new Bitmap(500, 500);
            
            DrawCurve(bitmap, curve);
            DrawCurveVelocity(bitmap, curve);

            foreach (var point in points)
            {
                DrawPixel(bitmap, point);
                DrawPoint(bitmap, point);
            }

            bitmap.Save(TestContext.CurrentContext.Test.Name  + ".bmp");
        }
        
        [SupportedOSPlatform("windows")]
        private static void DrawVelocityNormalized(BezierCurve curve, List<Vector3> points)
        {
            var bitmap = new Bitmap(500, 500);
            
            DrawCurve(bitmap, curve);
            DrawCurveVelocityNormalized(bitmap, curve);

            foreach (var point in points)
            {
                DrawPixel(bitmap, point);
                DrawPoint(bitmap, point);
            }

            bitmap.Save(TestContext.CurrentContext.Test.Name  + ".bmp");
        }

        [SupportedOSPlatform("windows")]
        private static void DrawSignedCurvature(BezierCurve curve, List<Vector3> points)
        {
            var bitmap = new Bitmap(500, 500);
            
            DrawCurve(bitmap, curve);
            DrawCurveSignedCurvature(bitmap, curve);

            foreach (var point in points)
            {
                DrawPixel(bitmap, point);
                DrawPoint(bitmap, point);
            }

            bitmap.Save(TestContext.CurrentContext.Test.Name  + ".bmp");
        }
        
        [SupportedOSPlatform("windows")]
        private static void DrawSignedCurvatureNormalized(BezierCurve curve, List<Vector3> points)
        {
            var bitmap = new Bitmap(500, 500);
            
            DrawCurve(bitmap, curve);
            DrawCurveSignedCurvatureNormalized(bitmap, curve);

            foreach (var point in points)
            {
                DrawPixel(bitmap, point);
                DrawPoint(bitmap, point);
            }

            bitmap.Save(TestContext.CurrentContext.Test.Name  + ".bmp");
        }

        [SupportedOSPlatform("windows")]
        private static void DrawPixel(Image bitmap, Vector3 point)
        {
            var pointInt = GetRotatedPoint(bitmap, point);
            
            var brush = Brushes.Goldenrod;
            using var graphics = System.Drawing.Graphics.FromImage(bitmap);
            graphics.FillRectangle(brush, pointInt.x, pointInt.y, 1, 1);
        }

        [SupportedOSPlatform("windows")]
        private static void DrawPoint(Image bitmap, Vector3 point)
        {
            var pointInt = GetRotatedPoint(bitmap, point);

            using var graphics = System.Drawing.Graphics.FromImage(bitmap);
            var x = Math.Max(0, pointInt.x - 2);
            var y = Math.Max(0, pointInt.y - 2);
            graphics.FillRectangle(Brushes.Brown, x, y, 5, 5);
        }
        
        [SupportedOSPlatform("windows")]
        private static void DrawCross(Image bitmap, Vector3 point)
        {
            var pointInt = GetRotatedPoint(bitmap, point);

            var x11 = Math.Max(0, pointInt.x - 2);
            var y11 = Math.Max(0, pointInt.y - 2);
            var x12 = Math.Min(bitmap.Width - 1, pointInt.x + 2);
            var y12 = Math.Min(bitmap.Height - 1, pointInt.y + 2);
            
            var x21 = Math.Max(0, pointInt.x - 2);
            var y21 = Math.Min(bitmap.Height - 1, pointInt.y + 2);
            var x22 = Math.Min(bitmap.Width - 1, pointInt.x + 2);
            var y22 = Math.Max(0, pointInt.y - 2);
            
            var pen = new Pen(Brushes.Brown, 1);
            using var graphics = System.Drawing.Graphics.FromImage(bitmap);
            graphics.DrawLine(pen, x11, y11, x12, y12);
            graphics.DrawLine(pen, x21, y21, x22, y22);
        }

        [SupportedOSPlatform("windows")]
        private static void DrawCurve(Image bitmap, BezierCurve curve)
        {
            var pen = new Pen(Brushes.Goldenrod, 3);

            using var graphics = System.Drawing.Graphics.FromImage(bitmap);
            const float step = 1.0f / 50;
            for (var t = 0.0f; t < 1.0f; t += step)
            {
                var point1 = curve.GetPoint(t);
                var point2 = curve.GetPoint(t + step);
                var point1Int = GetRotatedPoint(bitmap, point1);
                var point2Int = GetRotatedPoint(bitmap, point2);
                    
                graphics.DrawLine(pen, point1Int.x, point1Int.y, point2Int.x, point2Int.y);
            }
        }
        
        [SupportedOSPlatform("windows")]
        private static void DrawCurveVelocity(Image bitmap, BezierCurve curve)
        {
            var pen = new Pen(Brushes.CornflowerBlue, 2);

            using var graphics = System.Drawing.Graphics.FromImage(bitmap);
            const float step = 1.0f / 5;
            for (var t = 0.0f; t <= 1.0f; t += step)
            {
                var point1 = curve.GetPoint(t);
                var point1Int = GetRotatedPoint(bitmap, point1);
                var velocity = curve.GetVelocity(t) + point1;
                var velocityInt = GetRotatedPoint(bitmap, velocity);
                    
                graphics.DrawLine(pen, point1Int.x, point1Int.y, velocityInt.x, velocityInt.y);
            }
        }
        
        [SupportedOSPlatform("windows")]
        private static void DrawCurveVelocityNormalized(Image bitmap, BezierCurve curve)
        {
            var pen = new Pen(Brushes.CornflowerBlue, 2);

            using var graphics = System.Drawing.Graphics.FromImage(bitmap);
            const float step = 1.0f / 5;
            for (var t = 0.0f; t <= 1.0f; t += step)
            {
                var point1 = curve.GetPoint(t);
                var point1Int = GetRotatedPoint(bitmap, point1);
                var velocity = curve.GetVelocity(t).normalized * 100 + point1;
                var velocityInt = GetRotatedPoint(bitmap, velocity);
                    
                graphics.DrawLine(pen, point1Int.x, point1Int.y, velocityInt.x, velocityInt.y);
            }
        }
        
        [SupportedOSPlatform("windows")]
        private static void DrawCurveSignedCurvature(Image bitmap, BezierCurve curve)
        {
            var pen = new Pen(Brushes.CornflowerBlue, 2);

            using var graphics = System.Drawing.Graphics.FromImage(bitmap);
            const float step = 1.0f / 5;
            for (var t = 0.0f; t <= 1.0f; t += step)
            {
                var curvature = curve.GetCurvature(t);
                var sign = Math.Sign(curvature);
                    
                var point1 = curve.GetPoint(t);
                var point1Int = GetRotatedPoint(bitmap, point1);
                var velocity = curve.GetVelocity(t);

                var curvatureVector = sign > 0 ?  Vector3.Cross(Vector3.up, velocity): Vector3.Cross(velocity, Vector3.up);
                curvatureVector = curvatureVector * Math.Abs(curvature) * 10 + point1;
                    
                var curvatureVectorInt = GetRotatedPoint(bitmap, curvatureVector);
                graphics.DrawLine(pen, point1Int.x, point1Int.y, curvatureVectorInt.x, curvatureVectorInt.y);
            }
        }
        
        [SupportedOSPlatform("windows")]
        private static void DrawCurveSignedCurvatureNormalized(Image bitmap, BezierCurve curve)
        {
            var pen = new Pen(Brushes.CornflowerBlue, 2);

            using var graphics = System.Drawing.Graphics.FromImage(bitmap);
            const float step = 1.0f / 5;
            for (var t = 0.0f; t <= 1.0f; t += step)
            {
                var curvature = curve.GetCurvature(t);
                var sign = Math.Sign(curvature);
                    
                var point1 = curve.GetPoint(t);
                var point1Int = GetRotatedPoint(bitmap, point1);
                var velocity = curve.GetVelocity(t);

                var curvatureVector = sign > 0 ? Vector3.Cross(Vector3.up, velocity): Vector3.Cross(velocity, Vector3.up);
                curvatureVector = curvatureVector.normalized * 40 + point1;
                    
                var curvatureVectorInt = GetRotatedPoint(bitmap, curvatureVector);
                graphics.DrawLine(pen, point1Int.x, point1Int.y, curvatureVectorInt.x, curvatureVectorInt.y);
            }
        }

        [SupportedOSPlatform("windows")]
        private static Vector3Int GetRotatedPoint(Image bitmap, Vector3 point)
        {
            var x = (int)point.x;
            var y = bitmap.Height - (int)point.z - 1;

            return new Vector3Int(x, y, 0);
        }
    }
}