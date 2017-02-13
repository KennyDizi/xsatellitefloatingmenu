using System;
using System.Collections.Generic;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace bottomtabbedpage.iOS.Liquid
{
    [Register("LiquidFloatingActionButton")]
    public class LiquidFloatingActionButton : UIControl
    {
        private readonly nfloat _internalRadiusRatio = 0.3571429f;
        private bool _enableShadow = true;
        private UIColor _color = UIColor.FromRGB(82, 112, 235);
        private readonly CAShapeLayer _plusLayer = new CAShapeLayer();
        private readonly CAShapeLayer _circleLayer = new CAShapeLayer();
        private nfloat _plusRotation = 0.0f;
        private readonly CircleLiquidBaseView _baseView = new CircleLiquidBaseView();
        private readonly UIView _liquidView = new UIView();
        private bool _touching;

        
        [Export("CellRadiusRatio")]
        public nfloat CellRadiusRatio { get; set; }

        
        [Export("AnimateStyle")]
        public AnimateStyle AnimateStyle
        {
            get { return _baseView.AnimateStyle; }
            set { _baseView.AnimateStyle = value; }
        }

        
        [Export("EnableShadow")]
        public bool EnableShadow
        {
            get { return _enableShadow; }
            set
            {
                _enableShadow = value;
                SetNeedsDisplay();
            }
        }

        public IEnumerable<LiquidFloatingCell> Cells { get; set; }

        
        [Export("Responsible")]
        public bool Responsible { get; set; }

        public bool IsClosed
        {
            get { return _plusRotation == 0; }
            set
            {
                if (value)
                    Close();
                else
                    Open();
            }
        }

        
        [Export("Color")]
        public UIColor Color
        {
            get { return _color; }
            set
            {
                _color = value;
                _baseView.Color = _color;
            }
        }

        public event EventHandler<CellSelectedEventArgs> CellSelected;

        public event EventHandler<FloatingMenuOpenEventArgs> OnMenuOpened;

        public LiquidFloatingActionButton()
        {
            Setup();
        }

        public LiquidFloatingActionButton(IntPtr handle)
            : base(handle)
        {
        }

        public LiquidFloatingActionButton(NSCoder coder)
            : base(coder)
        {
            Setup();
        }

        public LiquidFloatingActionButton(CGRect frame)
            : base(frame)
        {
            Setup();
        }

        public override void AwakeFromNib()
        {
            // ISSUE: reference to a compiler-generated method
            base.AwakeFromNib();
            Setup();
        }

        private void InsertCell(LiquidFloatingCell cell)
        {
            var nfloat1 = Frame.Width * CellRadiusRatio;
            var nfloat2 = (Frame.Width - nfloat1) / 2f;
            cell.Frame = new CGRect(nfloat2, nfloat2, nfloat1, nfloat1);
            cell.Color = Color;
            cell.ActionButton = this;
            // ISSUE: reference to a compiler-generated method
            InsertSubviewAbove(cell, _baseView);
        }

        private LiquidFloatingCell[] CellArray()
        {
            if (Cells != null)
                return Cells.ToArray();
            return new LiquidFloatingCell[0];
        }

        public void Open()
        {
            // ISSUE: reference to a compiler-generated method
            _plusLayer.AddAnimation(PlusKeyFrame(true), "plusRot");
            _plusRotation = CoreGraphicsExtensions.Pi * 0.25f;
            var cells = CellArray();
            foreach (var cell in cells)
                InsertCell(cell);
            _baseView.Open(cells);
            // ISSUE: reference to a compiler-generated method
            SetNeedsDisplay();
            //raise event open menu
            OnMenuOpened?.Invoke(this, new FloatingMenuOpenEventArgs(isOpen:true));
        }

        public void Close()
        {
            // ISSUE: reference to a compiler-generated method
            _plusLayer.AddAnimation(PlusKeyFrame(false), "plusRot");
            _plusRotation = 0;
            _baseView.Close(CellArray());
            // ISSUE: reference to a compiler-generated method
            SetNeedsDisplay();
            //raise event close menu
            OnMenuOpened?.Invoke(this, new FloatingMenuOpenEventArgs(isOpen: false));
        }

        public override void Draw(CGRect rect)
        {
            // ISSUE: reference to a compiler-generated method
            base.Draw(rect);
            DrawCircle();
            DrawShadow();
            DrawPlus(_plusRotation);
        }

        private void DrawCircle()
        {
            _circleLayer.Frame = new CGRect(CGPoint.Empty, Frame.Size);
            _circleLayer.CornerRadius = Frame.Width * 0.5f;
            _circleLayer.MasksToBounds = true;
            if (_touching && Responsible)
                _circleLayer.BackgroundColor = Color.White(0.5f).CGColor;
            else
                _circleLayer.BackgroundColor = Color.CGColor;
        }

        private void DrawPlus(nfloat rotation)
        {
            _plusLayer.Frame = new CGRect(CGPoint.Empty, Frame.Size);
            _plusLayer.LineCap = CAShapeLayer.CapRound;
            _plusLayer.StrokeColor = UIColor.White.CGColor;
            _plusLayer.LineWidth = 3f;
            _plusLayer.Path = PathPlus(rotation).CGPath;
        }

        private void DrawShadow()
        {
            if (!EnableShadow) return;
            _circleLayer.AppendShadow();
        }

        private UIBezierPath PathPlus(nfloat rotation)
        {
            var radius = Frame.Width * _internalRadiusRatio * 0.5f;
            var circleCenter = Center.Minus(Frame.Location);
            var cgPointArray = new[]
            {
                circleCenter.CirclePoint(radius, CoreGraphicsExtensions.Pi2 * 0.0f + rotation),
                circleCenter.CirclePoint(radius, CoreGraphicsExtensions.Pi2 * 1f + rotation),
                circleCenter.CirclePoint(radius, CoreGraphicsExtensions.Pi2 * 2f + rotation),
                circleCenter.CirclePoint(radius, CoreGraphicsExtensions.Pi2 * 3f + rotation)
            };
            var uiBezierPath = new UIBezierPath();
            // ISSUE: reference to a compiler-generated method
            uiBezierPath.MoveTo(cgPointArray[0]);
            // ISSUE: reference to a compiler-generated method
            uiBezierPath.AddLineTo(cgPointArray[2]);
            // ISSUE: reference to a compiler-generated method
            uiBezierPath.MoveTo(cgPointArray[1]);
            // ISSUE: reference to a compiler-generated method
            uiBezierPath.AddLineTo(cgPointArray[3]);
            return uiBezierPath;
        }

        private CAKeyFrameAnimation PlusKeyFrame(bool closed)
        {
            UIBezierPath[] uiBezierPathArray1;
            if (closed)
                uiBezierPathArray1 = new []
                {
                    PathPlus(CoreGraphicsExtensions.Pi * 0.0f),
                    PathPlus(CoreGraphicsExtensions.Pi * 0.125f),
                    PathPlus(CoreGraphicsExtensions.Pi * 0.25f)
                };
            else
                uiBezierPathArray1 = new[]
                {
                    PathPlus(CoreGraphicsExtensions.Pi * 0.25f),
                    PathPlus(CoreGraphicsExtensions.Pi * 0.125f),
                    PathPlus(CoreGraphicsExtensions.Pi * 0.0f)
                };
            var uiBezierPathArray2 = uiBezierPathArray1;
            var fromKeyPath = CAKeyFrameAnimation.GetFromKeyPath("path");
            // ISSUE: reference to a compiler-generated method
            fromKeyPath.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
            var values = uiBezierPathArray2.Select(x => x.CGPath).ToArray();
            fromKeyPath.SetValues(value: values);
            fromKeyPath.Duration = 0.5;
            fromKeyPath.RemovedOnCompletion = true;
            fromKeyPath.FillMode = CAFillMode.Forwards;
            return fromKeyPath;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            // ISSUE: reference to a compiler-generated method
            base.TouchesBegan(touches, evt);
            _touching = true;
            // ISSUE: reference to a compiler-generated method
            SetNeedsDisplay();
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            // ISSUE: reference to a compiler-generated method
            base.TouchesEnded(touches, evt);
            _touching = false;
            // ISSUE: reference to a compiler-generated method
            SetNeedsDisplay();
            DidTapped();
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            // ISSUE: reference to a compiler-generated method
            base.TouchesCancelled(touches, evt);
            _touching = false;
            // ISSUE: reference to a compiler-generated method
            SetNeedsDisplay();
        }

        public override UIView HitTest(CGPoint point, UIEvent uievent)
        {
            foreach (var cell in CellArray())
            {
                // ISSUE: reference to a compiler-generated method
                var point1 = cell.ConvertPointFromView(point, this);
                if (cell.Bounds.Contains(point1) && cell.UserInteractionEnabled)
                {
                    // ISSUE: reference to a compiler-generated method
                    return cell.HitTest(point1, uievent);
                }
            }
            // ISSUE: reference to a compiler-generated method
            return base.HitTest(point, uievent);
        }

        public override void LayoutSubviews()
        {
            // ISSUE: reference to a compiler-generated method
            base.LayoutSubviews();
            var cgRect = new CGRect(CGPoint.Empty, Frame.Size);
            _liquidView.Frame = cgRect;
            _baseView.Frame = cgRect;
        }

        private void Setup()
        {
            CellRadiusRatio = 0.75f;
            Cells = new List<LiquidFloatingCell>();
            BackgroundColor = UIColor.Clear;
            ClipsToBounds = false;
            Responsible = true;
            _baseView.Setup(this);
            // ISSUE: reference to a compiler-generated method
            AddSubview(_baseView);
            _liquidView.UserInteractionEnabled = false;
            // ISSUE: reference to a compiler-generated method
            AddSubview(_liquidView);
            // ISSUE: reference to a compiler-generated method
            _liquidView.Layer.AddSublayer(_circleLayer);
            // ISSUE: reference to a compiler-generated method
            _circleLayer.AddSublayer(_plusLayer);
        }

        private void DidTapped()
        {
            if (IsClosed)
                Open();
            else
                Close();
        }

        protected internal virtual void OnCellSelected(LiquidFloatingCell target)
        {
            var cellSelected = CellSelected;
            if (cellSelected == null)
                return;
            var index = Array.IndexOf(CellArray(), target);
            cellSelected(this, new CellSelectedEventArgs(target, index));
        }
    }
}