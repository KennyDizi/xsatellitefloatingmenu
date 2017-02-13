using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace bottomtabbedpage.iOS.Liquid
{
    public class LiquidFloatingCell : LiquittableCircle
    {
        private readonly nfloat _imageRatio = 0.5f;
        private readonly WeakReference<LiquidFloatingActionButton> _actionButton = new WeakReference<LiquidFloatingActionButton>(null);
        private UIColor _originalColor = UIColor.Clear;
        private UIImageView _imageView;

        public UIView View { get; private set; }

        public LiquidFloatingActionButton ActionButton
        {
            get
            {
                LiquidFloatingActionButton target;
                _actionButton.TryGetTarget(out target);
                return target;
            }
            internal set
            {
                _actionButton.SetTarget(value);
            }
        }

        public bool Responsible { get; set; }

        public LiquidFloatingCell(UIImage icon)
        {
            Setup(icon);
        }

        public LiquidFloatingCell(UIImage icon, nfloat imageRatio)
        {
            _imageRatio = imageRatio;
            Setup(icon);
        }

        public LiquidFloatingCell(UIView view)
        {
            SetupView(view);
        }

        public override void LayoutSubviews()
        {
            // ISSUE: reference to a compiler-generated method
            base.LayoutSubviews();
            if (_imageView == null)
                return;
            var nfloat1 = Frame.Width * _imageRatio;
            var nfloat2 = (Frame.Width - nfloat1) / 2f;
            _imageView.Frame = new CGRect(nfloat2, nfloat2, nfloat1, nfloat1);
        }

        private void Setup(UIImage image, UIColor tintColor = null)
        {
            _imageView = new UIImageView
            {
                Image = image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate),
                TintColor = tintColor ?? UIColor.White
            };
            // ISSUE: reference to a compiler-generated method
            SetupView(_imageView);
        }

        private void SetupView(UIView view)
        {
            View = view;
            Responsible = true;
            UserInteractionEnabled = false;
            // ISSUE: reference to a compiler-generated method
            AddSubview(view);
        }

        public void Update(nfloat key, bool open)
        {
            foreach (var subview in Subviews)
            {
                var nfloat = (nfloat)Math.Max(2f * (key * key - 0.5f), 0.0);
                subview.Alpha = !open ? -nfloat : nfloat;
            }
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            // ISSUE: reference to a compiler-generated method
            base.TouchesBegan(touches, evt);
            if (!Responsible)
                return;
            _originalColor = Color;
            Color = _originalColor.White(0.5f);
            // ISSUE: reference to a compiler-generated method
            SetNeedsDisplay();
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            // ISSUE: reference to a compiler-generated method
            base.TouchesCancelled(touches, evt);
            if (!Responsible)
                return;
            Color = _originalColor;
            // ISSUE: reference to a compiler-generated method
            SetNeedsDisplay();
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            // ISSUE: reference to a compiler-generated method
            base.TouchesEnded(touches, evt);
            Color = _originalColor;
            ActionButton?.OnCellSelected(this);
        }
    }
}