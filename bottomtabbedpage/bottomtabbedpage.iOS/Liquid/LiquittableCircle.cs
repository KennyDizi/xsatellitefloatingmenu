using System;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace bottomtabbedpage.iOS.Liquid
{
    public class LiquittableCircle : UIView
    {
        private CGPoint[] _points = new CGPoint[0];
        private readonly CAShapeLayer _circleLayer = new CAShapeLayer();
        private UIColor _color = UIColor.Red;

        public nfloat Radius => Frame.Width / 2f;

        public UIColor Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                DrawCircle();
            }
        }

        public LiquittableCircle()
        {
            DrawCircle();
            SetupLayer();
        }

        private void DrawCircle()
        {
            // ISSUE: reference to a compiler-generated method
            var uiBezierPath = UIBezierPath.FromOval(new CGRect(CGPoint.Empty, Frame.Size));
            _circleLayer.LineWidth = 3f;
            _circleLayer.FillColor = Color.CGColor;
            _circleLayer.Path = uiBezierPath.CGPath;
        }

        private void SetupLayer()
        {
            // ISSUE: reference to a compiler-generated method
            Layer.AddSublayer(_circleLayer);
            Opaque = false;
        }

        internal CGPoint CirclePoint(nfloat rad)
        {
            return Center.CirclePoint(Radius, rad);
        }

        public override void Draw(CGRect rect)
        {
            DrawCircle();
        }
    }
}
