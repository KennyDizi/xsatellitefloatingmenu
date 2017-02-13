using System;
using System.Collections.Generic;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace bottomtabbedpage.iOS.Liquid
{
    internal class CircleLiquidBaseView : UIView
    {
        private readonly nfloat _openDuration = 0.6f;
        private readonly nfloat _closeDuration = 0.2f;
        private readonly nfloat _viscosity = 0.65f;
        private UIColor _color = UIColor.FromRGB(82, 112, 235);
        private readonly List<LiquidFloatingCell> _openingCells = new List<LiquidFloatingCell>();
        private nfloat _keyDuration = 0;
        private bool _opening;
        public AnimateStyle AnimateStyle;
        private LiquittableCircle _baseLiquid;
        private SimpleCircleLiquidEngine _engine;
        private SimpleCircleLiquidEngine _bigEngine;
        private CADisplayLink _displayLink;

        public UIColor Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                if (_engine != null)
                    _engine.Color = _color;
                if (_bigEngine == null)
                    return;
                _bigEngine.Color = _color;
            }
        }

        public void Setup(LiquidFloatingActionButton actionButton)
        {
            AnimateStyle = actionButton.AnimateStyle;
            ClipsToBounds = false;
            Layer.MasksToBounds = false;
            _engine = new SimpleCircleLiquidEngine
            {
                Viscosity = _viscosity,
                Color = actionButton.Color
            };
            _bigEngine = new SimpleCircleLiquidEngine
            {
                Viscosity = _viscosity,
                Color = actionButton.Color
            };
            _baseLiquid = new LiquittableCircle
            {
                Color = actionButton.Color,
                ClipsToBounds = false
            };
            _baseLiquid.Layer.MasksToBounds = false;
            // ISSUE: reference to a compiler-generated method
            AddSubview(_baseLiquid);
        }

        public override void LayoutSubviews()
        {
            // ISSUE: reference to a compiler-generated method
            base.LayoutSubviews();
            var nfloat = (nfloat)Math.Min(Frame.Width, Frame.Height) * 0.5f;
            _engine.RadiusThreshold = nfloat * 0.73f;
            _engine.AngleThreshold = 0.45f;
            _bigEngine.RadiusThreshold = nfloat;
            _bigEngine.AngleThreshold = 0.55f;
            _baseLiquid.Frame = new CGRect(CGPoint.Empty, Frame.Size);
        }

        public void Open(LiquidFloatingCell[] cells)
        {
            Stop();
            _displayLink = CADisplayLink.Create(DidDisplayRefresh);
            _displayLink.AddToRunLoop(NSRunLoop.Current, NSRunLoopMode.Common);
            _opening = true;
            foreach (var cell in cells)
            {
                // ISSUE: reference to a compiler-generated method
                cell.Layer.RemoveAllAnimations();
                cell.Layer.EraseShadow();
                _openingCells.Add(cell);
            }
        }

        public void Close(LiquidFloatingCell[] cells)
        {
            Stop();
            _opening = false;
            _displayLink = CADisplayLink.Create(DidDisplayRefresh);
            _displayLink.AddToRunLoop(NSRunLoop.Current, NSRunLoopMode.Common);
            foreach (var cell in cells)
            {
                // ISSUE: reference to a compiler-generated method
                cell.Layer.RemoveAllAnimations();
                cell.Layer.EraseShadow();
                _openingCells.Add(cell);
                cell.UserInteractionEnabled = false;
            }
        }

        private void DidFinishUpdate()
        {
            if (_opening)
            {
                foreach (var openingCell in _openingCells)
                    openingCell.UserInteractionEnabled = true;
            }
            else
            {
                foreach (var openingCell in _openingCells)
                {
                    // ISSUE: reference to a compiler-generated method
                    openingCell.RemoveFromSuperview();
                }
            }
        }

        private void Update(nfloat delay, nfloat duration, Action<LiquidFloatingCell, int, nfloat> f)
        {
            if (_openingCells.Count == 0)
                return;
            var nfloat1 = duration + _openingCells.Count * delay;
            var keyDuration = _keyDuration;
            if (EaseInEaseOut(keyDuration / nfloat1) >= 1f)
            {
                DidFinishUpdate();
                Stop();
            }
            else
            {
                _engine.Clear();
                _bigEngine.Clear();
                for (var index = 0; index < _openingCells.Count; ++index)
                {
                    var openingCell = _openingCells[index];
                    var nfloat2 = delay * index;
                    var nfloat3 = EaseInEaseOut((keyDuration - nfloat2) / duration);
                    f(openingCell, index, nfloat3);
                }
                var liquidFloatingCell = _openingCells.FirstOrDefault();
                if (liquidFloatingCell != null)
                    _bigEngine.Push(_baseLiquid, liquidFloatingCell);
                for (var index = 1; index < _openingCells.Count; ++index)
                    _engine.Push(_openingCells[index - 1], _openingCells[index]);
                _engine.Draw(_baseLiquid);
                _bigEngine.Draw(_baseLiquid);
            }
        }

        private void UpdateOpen()
        {
            Update(0.1f, _openDuration, (cell, i, ratio) =>
            {
                var nfloat = !(ratio > i / (nfloat)_openingCells.Count) ? 0 : ratio;
                var distance = (cell.Frame.Height * 0.5f + (i + 1f) * cell.Frame.Height * 1.5f) * nfloat;
                cell.Center = Center.Plus(DifferencePoint(distance));
                cell.Update(ratio, true);
            });
        }

        private void UpdateClose()
        {
            Update(0.0f, _closeDuration, (cell, i, ratio) =>
            {
                var distance = (cell.Frame.Height * 0.5f + (i + 1f) * cell.Frame.Height * 1.5f) * (1f - ratio);
                cell.Center = Center.Plus(DifferencePoint(distance));
                cell.Update(ratio, false);
            });
        }

        private CGPoint DifferencePoint(nfloat distance)
        {
            switch (AnimateStyle)
            {
                case AnimateStyle.Right:
                    return new CGPoint(distance, 0);
                case AnimateStyle.Left:
                    return new CGPoint(-distance, 0);
                case AnimateStyle.Down:
                    return new CGPoint(0, distance);
                default:
                    return new CGPoint(0, -distance);
            }
        }

        private void Stop()
        {
            foreach (var openingCell in _openingCells)
            {
                openingCell.Layer.AppendShadow();
            }
            _openingCells.Clear();
            _keyDuration = 0;
            // ISSUE: reference to a compiler-generated method
            _displayLink?.Invalidate();
        }

        private static nfloat EaseInEaseOut(nfloat t)
        {
            if (t >= 1f)
                return 1f;
            if (t < 0.0f)
                return 0.0f;
            var nfloat = t * 2f;
            return nfloat - 1f * t * (t - 2f);
        }

        private void DidDisplayRefresh()
        {
            _keyDuration += (nfloat)_displayLink.Duration;
            if (_opening)
                UpdateOpen();
            else
                UpdateClose();
        }
    }


    internal static class CoreGraphicsExtensions
    {
        public static nfloat Pi = (nfloat)Math.PI;
        public static nfloat Pi2 = (nfloat)Math.PI / 2f;

        public static nfloat RadToDeg(nfloat rad)
        {
            return rad * 180f / Pi;
        }

        public static nfloat DegToRad(nfloat deg)
        {
            return deg * Pi / 180f;
        }

        public static nfloat[] LinSpace(nfloat from, nfloat to, int n)
        {
            var nfloatArray = new nfloat[n];
            for (var index = 0; index < n; ++index)
                nfloatArray[index] = (to - from) * index / (n - 1) + from;
            return nfloatArray;
        }

        public static void AppendShadow(this CALayer layer)
        {
            layer.ShadowColor = UIColor.Black.CGColor;
            layer.ShadowRadius = 2f;
            layer.ShadowOpacity = 0.1f;
            layer.ShadowOffset = new CGSize(4, (nfloat)4);
            layer.MasksToBounds = false;
        }

        public static void EraseShadow(this CALayer layer)
        {
            layer.ShadowRadius = 0.0f;
            layer.ShadowColor = UIColor.Clear.CGColor;
        }

        public static CGPoint CirclePoint(this CGPoint circleCenter, nfloat radius, nfloat rad)
        {
            return new CGPoint(circleCenter.X + radius * (nfloat)Math.Cos(rad), circleCenter.Y + radius * (nfloat)Math.Sin(rad));
        }

        public static CGPoint Plus(this CGPoint self, CGPoint point)
        {
            return new CGPoint(self.X + point.X, self.Y + point.Y);
        }

        public static CGPoint Minus(this CGPoint self, CGPoint point)
        {
            return new CGPoint(self.X - point.X, self.Y - point.Y);
        }

        public static CGPoint MinusX(this CGPoint self, nfloat dx)
        {
            return new CGPoint(self.X - dx, self.Y);
        }

        public static CGPoint MinusY(this CGPoint self, nfloat dy)
        {
            return new CGPoint(self.X, self.Y - dy);
        }

        public static CGPoint Mul(this CGPoint self, nfloat rhs)
        {
            return new CGPoint(self.X * rhs, self.Y * rhs);
        }

        public static CGPoint Div(this CGPoint self, nfloat rhs)
        {
            return new CGPoint(self.X / rhs, self.Y / rhs);
        }

        public static nfloat Length(this CGPoint self)
        {
            return (nfloat)Math.Sqrt(self.X * self.X + self.Y * self.Y);
        }

        public static CGPoint Normalized(this CGPoint self)
        {
            return self.Div(self.Length());
        }

        public static nfloat Dot(this CGPoint self, CGPoint point)
        {
            return self.X * point.X + self.Y * point.Y;
        }

        public static nfloat Cross(this CGPoint self, CGPoint point)
        {
            return self.X * point.Y - self.Y * point.X;
        }

        public static CGPoint Split(this CGPoint self, CGPoint point, nfloat ratio)
        {
            return self.Mul(ratio).Plus(point.Mul(1f - ratio));
        }

        public static CGPoint Mid(this CGPoint self, CGPoint point)
        {
            return self.Split(point, 0.5f);
        }

        public static CGPoint? Intersection(CGPoint from, CGPoint to, CGPoint from2, CGPoint to2)
        {
            var cgPoint = new CGPoint(to.X - from.X, to.Y - from.Y);
            var self = new CGPoint(to2.X - from2.X, to2.Y - from2.Y);
            var point1 = new CGPoint(from2.X - from.X, from2.Y - from.Y);
            var point2 = new CGPoint(to.X - from2.X, to.Y - from2.Y);
            var nfloat1 = self.Cross(point1);
            var nfloat2 = self.Cross(point2);
            if (Math.Abs(nfloat1 + nfloat2) < 0.1)
                return new CGPoint?();
            var nfloat3 = nfloat1 / (nfloat1 + nfloat2);
            return new CGPoint(from.X + nfloat3 * cgPoint.X, from.Y + nfloat3 * cgPoint.Y);
        }

        public static UIColor White(this UIColor self, nfloat scale)
        {
            nfloat red;
            nfloat green;
            nfloat blue;
            nfloat alpha;
            self.GetRGBA(out red, out green, out blue, out alpha);
            return new UIColor(red + (1f - red) * scale, green + (1f - green) * scale, blue + (1f - blue) * scale, 1f);
        }
    }
}