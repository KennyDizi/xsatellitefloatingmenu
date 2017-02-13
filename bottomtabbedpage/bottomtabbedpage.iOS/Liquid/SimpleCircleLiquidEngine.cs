using System;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace bottomtabbedpage.iOS.Liquid
{
    internal class SimpleCircleLiquidEngine
    {
        private readonly nfloat _connectThresh;
        private readonly nfloat _angleOpen;
        private CALayer _layer;

        public nfloat AngleThreshold { get; set; }

        public nfloat RadiusThreshold { get; set; }

        public UIColor Color { get; set; }

        public nfloat Viscosity { get; set; }

        public SimpleCircleLiquidEngine()
        {
            _layer = new CAShapeLayer();
            _connectThresh = 0.3f;
            _angleOpen = 1f;
            Viscosity = 0.65f;
            Color = UIColor.Red;
        }

        public LiquittableCircle[] Push(LiquittableCircle circle, LiquittableCircle other)
        {
            var connectedPath = GenerateConnectedPath(circle, other);
            if (connectedPath == null)
                return new LiquittableCircle[0];
            foreach (var layer in connectedPath.Select(ConstructLayer))
            {
                // ISSUE: reference to a compiler-generated method
                _layer.AddSublayer(layer);
            }
            return new[] {circle, other};
        }

        public void Draw(UIView parent)
        {
            // ISSUE: reference to a compiler-generated method
            parent.Layer.AddSublayer(_layer);
        }

        public void Clear()
        {
            // ISSUE: reference to a compiler-generated method
            _layer.RemoveFromSuperLayer();
            if (_layer.Sublayers != null)
            {
                foreach (var sublayer in _layer.Sublayers)
                {
                    // ISSUE: reference to a compiler-generated method
                    sublayer.RemoveFromSuperLayer();
                }
            }
            _layer = new CAShapeLayer();
        }

        private CALayer ConstructLayer(UIBezierPath path)
        {
            var boundingBox = path.CGPath.BoundingBox;
            var caShapeLayer = new CAShapeLayer
            {
                FillColor = Color.CGColor,
                Path = path.CGPath,
                Frame = new CGRect(0, 0, boundingBox.Width, boundingBox.Height)
            };
            return caShapeLayer;
        }

        private static Tuple<CGPoint, CGPoint> CircleConnectedPoint(LiquittableCircle circle, LiquittableCircle other,
            nfloat angle)
        {
            var cgPoint = other.Center.Minus(circle.Center);
            var nfloat = (nfloat) Math.Atan2(cgPoint.Y, cgPoint.X);
            return new Tuple<CGPoint, CGPoint>(circle.Center.CirclePoint(circle.Radius, nfloat + angle),
                circle.Center.CirclePoint(circle.Radius, nfloat - angle));
        }

        private Tuple<CGPoint, CGPoint> CircleConnectedPoint(LiquittableCircle circle, LiquittableCircle other)
        {
            var nfloat = (CircleRatio(circle, other) + _connectThresh) / (1f + _connectThresh);
            var angle = CoreGraphicsExtensions.Pi2 * _angleOpen * nfloat;
            return CircleConnectedPoint(circle, other, angle);
        }

        private UIBezierPath[] GenerateConnectedPath(LiquittableCircle circle, LiquittableCircle other)
        {
            if (!IsConnected(circle, other))
                return null;
            var ratio = CircleRatio(circle, other);
            if (ratio >= AngleThreshold && ratio <= 1f)
            {
                var uiBezierPath = NormalPath(circle, other);
                if (uiBezierPath == null)
                    return null;
                return new[] {uiBezierPath};
            }
            if (ratio >= 0.0f && ratio < AngleThreshold)
                return SplitPath(circle, other, ratio);
            return null;
        }

        private UIBezierPath NormalPath(LiquittableCircle circle, LiquittableCircle other)
        {
            var tuple1 = CircleConnectedPoint(circle, other);
            var tuple2 = CircleConnectedPoint(other, circle);
            var cgPoint1 = tuple1.Item1;
            var cgPoint2 = tuple1.Item2;
            var cgPoint3 = tuple2.Item1;
            var cgPoint4 = tuple2.Item2;
            var nullable = CoreGraphicsExtensions.Intersection(cgPoint1, cgPoint3, cgPoint2, cgPoint4);
            if (!nullable.HasValue)
                return null;
            var uiBezierPath = new UIBezierPath();
            var nfloat = CircleRatio(circle, other);
            // ISSUE: reference to a compiler-generated method
            uiBezierPath.MoveTo(cgPoint1);
            var cgPoint5 = cgPoint2.Mid(cgPoint3);
            var cgPoint6 = cgPoint1.Mid(cgPoint4);
            var ratio = (1 - nfloat) / (1 - AngleThreshold) * Viscosity;
            var controlPoint1 = cgPoint5.Mid(nullable.Value).Split(cgPoint6, ratio);
            var controlPoint2 = cgPoint6.Mid(nullable.Value).Split(cgPoint5, ratio);
            // ISSUE: reference to a compiler-generated method
            uiBezierPath.AddQuadCurveToPoint(cgPoint4, controlPoint1);
            // ISSUE: reference to a compiler-generated method
            uiBezierPath.AddLineTo(cgPoint3);
            // ISSUE: reference to a compiler-generated method
            uiBezierPath.AddQuadCurveToPoint(cgPoint2, controlPoint2);
            // ISSUE: reference to a compiler-generated method
            uiBezierPath.ClosePath();
            return uiBezierPath;
        }

        private UIBezierPath[] SplitPath(LiquittableCircle circle, LiquittableCircle other, nfloat ratio)
        {
            var tuple1 = CircleConnectedPoint(circle, other, CoreGraphicsExtensions.DegToRad(60));
            var tuple2 = CircleConnectedPoint(other, circle, CoreGraphicsExtensions.DegToRad(60));
            var cgPoint1 = tuple1.Item1;
            var cgPoint2 = tuple1.Item2;
            var cgPoint3 = tuple2.Item1;
            var cgPoint4 = tuple2.Item2;
            var nullable = CoreGraphicsExtensions.Intersection(cgPoint1, cgPoint3, cgPoint2, cgPoint4);
            if (!nullable.HasValue)
                return new UIBezierPath[0];
            var self1 = CircleConnectedPoint(circle, other, 0).Item1;
            var self2 = CircleConnectedPoint(other, circle, 0).Item1;
            var nfloat = (ratio - _connectThresh) / (AngleThreshold - _connectThresh);
            var controlPoint1 = self2.Split(nullable.Value, nfloat * nfloat);
            var uiBezierPath1 = new UIBezierPath();
            // ISSUE: reference to a compiler-generated method
            uiBezierPath1.MoveTo(cgPoint1);
            // ISSUE: reference to a compiler-generated method
            uiBezierPath1.AddQuadCurveToPoint(cgPoint2, controlPoint1);
            // ISSUE: reference to a compiler-generated method
            uiBezierPath1.ClosePath();
            var controlPoint2 = self1.Split(nullable.Value, nfloat * nfloat);
            var uiBezierPath2 = new UIBezierPath();
            // ISSUE: reference to a compiler-generated method
            uiBezierPath2.MoveTo(cgPoint3);
            // ISSUE: reference to a compiler-generated method
            uiBezierPath2.AddQuadCurveToPoint(cgPoint4, controlPoint2);
            // ISSUE: reference to a compiler-generated method
            uiBezierPath2.ClosePath();
            return new[]
            {
                uiBezierPath1,
                uiBezierPath2
            };
        }

        private nfloat CircleRatio(LiquittableCircle circle, LiquittableCircle other)
        {
            return
                (nfloat)
                Math.Min(
                    Math.Max(
                        1f -
                        (other.Center.Minus(circle.Center).Length() - RadiusThreshold) /
                        (circle.Radius + other.Radius + RadiusThreshold), 0.0), 1.0);
        }

        private bool IsConnected(LiquittableCircle circle, LiquittableCircle other)
        {
            return circle.Center.Minus(other.Center).Length() - circle.Radius - other.Radius < RadiusThreshold;
        }
    }
}