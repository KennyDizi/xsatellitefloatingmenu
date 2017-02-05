using System;
using System.Collections.Generic;
using System.Drawing;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace bottomtabbedpage.iOS.Satellite
{
    public sealed class SatelliteMenuButton : UIButton
    {
        private readonly List<UIButton> _buttonItems;
        private readonly List<SatelliteMenuButtonItem> _menuItems;
        private readonly UIView _parentView;
        private bool _expanded;
        private bool _transition;

        public double Radius { get; set; }

        public double Speed { get; set; }

        public double Bounce { get; set; }

        public double BounceSpeed { get; set; }

        public bool RotateToSide { get; set; }

        public bool CloseItemsOnClick { get; set; }

        public SatelliteMenuButtonItem[] Items => _menuItems.ToArray();

        public event EventHandler<SatelliteMenuItemEventArgs> MenuItemClick;

        public SatelliteMenuButton(IEnumerable<UIButton> buttonItems, RectangleF rect)
            : base(rect)
        {
            _buttonItems = new List<UIButton>(buttonItems);
            Initialize();
        }

        public SatelliteMenuButton(UIView parentView, UIImage menuButtonImage, SatelliteMenuButtonItem[] menuItems,
            CGRect rect)
            : this(parentView, menuButtonImage, rect)
        {
            AddItems(menuItems);
        }

        public SatelliteMenuButton(UIView parentView, UIImage menuButtonImage, CGRect rect)
            : base(rect)
        {
            _parentView = parentView;
            // ISSUE: reference to a compiler-generated method
            SetBackgroundImage(menuButtonImage, UIControlState.Normal);
            _buttonItems = new List<UIButton>();
            _menuItems = new List<SatelliteMenuButtonItem>();
            Initialize();
        }

        private void Initialize()
        {
            Radius = 140.0;
            Speed = 0.2;
            Bounce = 0.3;
            BounceSpeed = 0.2;
            RotateToSide = true;
            CloseItemsOnClick = true;
            _expanded = false;
            _transition = false;
            foreach (var buttonItem in _buttonItems)
            {
                buttonItem.Alpha = 0;
                buttonItem.Center = Center;
            }
            TouchUpInside += HandleTouchUpInside;
        }

        public void AddItem(UIImage itemImage)
        {
            AddItem(new SatelliteMenuButtonItem(itemImage));
        }

        public void AddItem(UIImage itemImage, int tag)
        {
            AddItem(new SatelliteMenuButtonItem(itemImage, tag));
        }

        public void AddItem(UIImage itemImage, string name)
        {
            AddItem(new SatelliteMenuButtonItem(itemImage, name));
        }

        public void AddItem(UIImage itemImage, int tag, string name)
        {
            AddItem(new SatelliteMenuButtonItem(itemImage, tag, name));
        }

        public void AddItems(IEnumerable<SatelliteMenuButtonItem> menuItem)
        {
            foreach (var menuItem1 in menuItem)
                AddItem(menuItem1);
        }

        public void AddItems(params SatelliteMenuButtonItem[] menuItem)
        {
            foreach (var menuItem1 in menuItem)
                AddItem(menuItem1);
        }

        public void AddItem(SatelliteMenuButtonItem menuItem)
        {
            _menuItems.Add(menuItem);
            var button =
                new UIButton(new CGRect(0, 0, menuItem.ItemImage.CGImage.Width / UIScreen.MainScreen.Scale,
                    menuItem.ItemImage.CGImage.Height / UIScreen.MainScreen.Scale));
            button.SetBackgroundImage(menuItem.ItemImage, 0L);
            menuItem.ImageChanged = delegate
            {
                button.SetBackgroundImage(menuItem.ItemImage, 0L);
            };

            button.TouchUpInside += HandleTouchUpInsideButton;
            _buttonItems.Add(button);
            _parentView.AddSubview(button);
            button.Center = Center;
        }

        public void RemoveItem(SatelliteMenuButtonItem menuItem)
        {
            var index = _menuItems.IndexOf(menuItem);
            var buttonItem = _buttonItems[index];
            // ISSUE: reference to a compiler-generated method
            buttonItem.RemoveFromSuperview();
            buttonItem.TouchUpInside -= HandleTouchUpInsideButton;
            buttonItem.Dispose();
            menuItem.ImageChanged = null;
            _buttonItems.RemoveAt(index);
            _menuItems.RemoveAt(index);
        }

        public void Expand()
        {
            if (_transition)
            {
                return;
            }
            _transition = true;
            if (RotateToSide)
            {
                Animate(Speed, delegate
                {
                    Transform = CGAffineTransform.MakeRotation(0.7853982f);
                });
            }

            var buttomItemCount = _buttonItems.Count;
            for (var index = 0; index < buttomItemCount; index++)
            {
                var button = _buttonItems[index];
                button.Superview.BringSubviewToFront(button);
                var num = buttomItemCount > 1 ? 1.0 / (buttomItemCount - 1) : 1.0;
                var num2 = index * num;
                var num3 = (1.0 - num2) * 90.0 * 3.1415927410125732 / 180.0;
                var rotation = CGAffineTransform.MakeRotation((float)num3);
                var num4 = (Radius + Bounce * Radius) * rotation.xx;
                var num5 = (Radius + Bounce * Radius) * rotation.xy;
                var center = new PointF((float)(button.Center.X + num4), (float)(button.Center.Y + num5));
                var cABasicAnimation = CABasicAnimation.FromKeyPath("transform.rotation");
                cABasicAnimation.To = NSNumber.FromDouble(12.566370614359172);
                cABasicAnimation.Duration = 0.4;
                cABasicAnimation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseOut);
                button.Layer.AddAnimation(cABasicAnimation, "spinAnimation");
                var index1 = index;
                Animate(Speed, Speed * num2, UIViewAnimationOptions.CurveEaseIn, delegate
                {
                    button.Center = center;
                    button.Alpha = 1.0f;
                }, delegate
                {
                    Animate(BounceSpeed, delegate
                    {
                        var num6 = Bounce * Radius * rotation.xx;
                        var num7 = Bounce * Radius * rotation.xy;
                        var pointF = new PointF((float)(button.Center.X - num6), (float)(button.Center.Y - num7));
                        button.Center = pointF;
                    });
                    if (index1 == buttomItemCount)
                    {
                        _expanded = true;
                        _transition = false;
                    }
                });
            }
        }

        public void Collapse()
        {
            if (_transition)
            {
                return;
            }
            _transition = true;
            Animate(Speed, delegate
            {
                Transform = CGAffineTransform.MakeIdentity();
            });
            var buttomItemCount = _buttonItems.Count;
            for (var index = 0; index < buttomItemCount; index++)
            {
                var button = _buttonItems[index];
                var num = buttomItemCount > 1 ? 1.0 / (buttomItemCount - 1) : 1.0;
                var num2 = index * num;
                var index1 = index;
                Animate(Speed, Speed * (1.0 - num2), UIViewAnimationOptions.CurveEaseOut, delegate
                {
                    button.Alpha = 0.0f;
                    button.Center = Center;
                    button.Transform = CGAffineTransform.MakeIdentity();
                }, delegate
                {
                    if (index1 == buttomItemCount)
                    {
                        _expanded = false;
                        _transition = false;
                    }
                });
            }
        }

        public override void LayoutSubviews()
        {
            // ISSUE: reference to a compiler-generated method
            base.LayoutSubviews();
            Collapse();
        }

        private void HandleTouchUpInside(object sender, EventArgs e)
        {
            if (_transition)
                return;
            if (!_expanded)
                Expand();
            else
                Collapse();
        }

        private void HandleTouchUpInsideButton(object sender, EventArgs e)
        {
            var button = sender as UIButton;
            var satelliteMenuButtonItem = _menuItems[_buttonItems.IndexOf(button)];
            satelliteMenuButtonItem.FireClick(satelliteMenuButtonItem, EventArgs.Empty);
            MenuItemClick?.Invoke(sender, new SatelliteMenuItemEventArgs
            {
                MenuItem = satelliteMenuButtonItem
            });
            Animate(0.15, delegate
            {
                if (button != null) button.Transform = CGAffineTransform.MakeScale(1.2f, 1.2f);
            }, delegate
            {
                if (CloseItemsOnClick)
                {
                    Collapse();
                }
            });
        }
    }
}